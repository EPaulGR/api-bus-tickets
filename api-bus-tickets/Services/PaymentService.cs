using api_bus_tickets.Data;
using api_bus_tickets.DTOs;
using api_bus_tickets.Models;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace api_bus_tickets.Services
{
    public interface IPaymentService
    {
        Task<PaymentIntentDto> CreatePaymentIntentAsync(int ticketId, int userId);
        Task<PaymentDto> ProcessPaymentAsync(CreatePaymentDto createPaymentDto, int userId);
        Task<PaymentDto?> GetPaymentByIdAsync(int id);
        Task<IEnumerable<PaymentDto>> GetPaymentsByUserAsync(int userId);
        Task<bool> RefundPaymentAsync(int paymentId);
    }

    public class PaymentService : IPaymentService
    {
        private readonly ApplicationDbContext _context;
        private readonly ITicketService _ticketService;
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public PaymentService(
            ApplicationDbContext context,
            ITicketService ticketService,
            IUserService userService,
            IConfiguration configuration)
        {
            _context = context;
            _ticketService = ticketService;
            _userService = userService;
            _configuration = configuration;

            StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];
        }

        public async Task<PaymentIntentDto> CreatePaymentIntentAsync(int ticketId, int userId)
        {
            var ticket = await _ticketService.GetTicketByIdAsync(ticketId);
            if (ticket == null)
                throw new InvalidOperationException("Ticket not found");

            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
                throw new InvalidOperationException("User not found");

            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)(ticket.Price * 100), // Stripe works with cents
                Currency = "usd",
                Customer = user.StripeCustomerId,
                Metadata = new Dictionary<string, string>
                {
                    { "TicketId", ticketId.ToString() },
                    { "UserId", userId.ToString() }
                }
            };

            var service = new PaymentIntentService();
            var intent = await service.CreateAsync(options);

            return new PaymentIntentDto
            {
                ClientSecret = intent.ClientSecret,
                PaymentIntentId = intent.Id
            };
        }

        public async Task<PaymentDto> ProcessPaymentAsync(CreatePaymentDto createPaymentDto, int userId)
        {
            var ticket = await _ticketService.GetTicketByIdAsync(createPaymentDto.TicketId);
            if (ticket == null)
                throw new InvalidOperationException("Ticket not found");

            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
                throw new InvalidOperationException("User not found");

            var payment = new Payment
            {
                TicketId = createPaymentDto.TicketId,
                UserId = userId,
                Amount = ticket.Price,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            try
            {
                var options = new PaymentIntentConfirmOptions
                {
                    PaymentMethod = createPaymentDto.PaymentMethodId
                };

                var service = new PaymentIntentService();
                var intent = await service.ConfirmAsync(createPaymentDto.PaymentIntentId, options);

                payment.Status = intent.Status == "succeeded" ? "Completed" : "Failed";
                payment.StripePaymentIntentId = intent.Id;
                payment.StripeCustomerId = user.StripeCustomerId;
                payment.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                if (payment.Status == "Completed")
                {
                    // Actualizar el estado del ticket
                    await _ticketService.UpdateTicketStatusAsync(ticket.Id, "Paid");
                }
            }
            catch (Exception)
            {
                payment.Status = "Failed";
                payment.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                throw;
            }

            return new PaymentDto
            {
                Id = payment.Id,
                TicketId = payment.TicketId,
                UserId = payment.UserId,
                Amount = payment.Amount,
                Currency = payment.Currency,
                Status = payment.Status,
                CreatedAt = payment.CreatedAt,
                UpdatedAt = payment.UpdatedAt
            };
        }

        public async Task<PaymentDto?> GetPaymentByIdAsync(int id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null) return null;

            return new PaymentDto
            {
                Id = payment.Id,
                TicketId = payment.TicketId,
                UserId = payment.UserId,
                Amount = payment.Amount,
                Currency = payment.Currency,
                Status = payment.Status,
                CreatedAt = payment.CreatedAt,
                UpdatedAt = payment.UpdatedAt
            };
        }

        public async Task<IEnumerable<PaymentDto>> GetPaymentsByUserAsync(int userId)
        {
            return await _context.Payments
                .Where(p => p.UserId == userId)
                .Select(p => new PaymentDto
                {
                    Id = p.Id,
                    TicketId = p.TicketId,
                    UserId = p.UserId,
                    Amount = p.Amount,
                    Currency = p.Currency,
                    Status = p.Status,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt
                })
                .ToListAsync();
        }

        public async Task<bool> RefundPaymentAsync(int paymentId)
        {
            var payment = await _context.Payments.FindAsync(paymentId);
            if (payment == null || payment.Status != "Completed")
                return false;

            try
            {
                var options = new RefundCreateOptions
                {
                    PaymentIntent = payment.StripePaymentIntentId
                };

                var service = new RefundService();
                await service.CreateAsync(options);

                payment.Status = "Refunded";
                payment.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                // Actualizar el estado del ticket
                await _ticketService.UpdateTicketStatusAsync(payment.TicketId, "Refunded");

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
} 