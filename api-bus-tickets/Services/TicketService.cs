using api_bus_tickets.Data;
using api_bus_tickets.DTOs;
using api_bus_tickets.Models;
using Microsoft.EntityFrameworkCore;

namespace api_bus_tickets.Services
{
    public class TicketService : ITicketService
    {
        private readonly ApplicationDbContext _context;
        private readonly IScheduleService _scheduleService;

        public TicketService(ApplicationDbContext context, IScheduleService scheduleService)
        {
            _context = context;
            _scheduleService = scheduleService;
        }

        public async Task<IEnumerable<TicketDto>> GetAllTicketsAsync()
        {
            return await _context.Tickets
                .Include(t => t.Schedule)
                    .ThenInclude(s => s!.Route)
                .Include(t => t.Schedule)
                    .ThenInclude(s => s!.Bus)
                .Include(t => t.User)
                .Select(t => new TicketDto
                {
                    Id = t.Id,
                    TicketNumber = t.TicketNumber,
                    ScheduleId = t.ScheduleId,
                    UserId = t.UserId,
                    SeatNumber = t.SeatNumber,
                    Price = t.Price,
                    Status = t.Status,
                    PurchaseDate = t.PurchaseDate,
                    UserName = t.User!.Username,
                    RouteOrigin = t.Schedule!.Route!.Origin,
                    RouteDestination = t.Schedule!.Route!.Destination,
                    DepartureTime = t.Schedule!.DepartureTime
                })
                .ToListAsync();
        }

        public async Task<TicketDto?> GetTicketByIdAsync(int id)
        {
            var ticket = await _context.Tickets
                .Include(t => t.Schedule)
                    .ThenInclude(s => s!.Route)
                .Include(t => t.Schedule)
                    .ThenInclude(s => s!.Bus)
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (ticket == null) return null;

            return new TicketDto
            {
                Id = ticket.Id,
                TicketNumber = ticket.TicketNumber,
                ScheduleId = ticket.ScheduleId,
                UserId = ticket.UserId,
                SeatNumber = ticket.SeatNumber,
                Price = ticket.Price,
                Status = ticket.Status,
                PurchaseDate = ticket.PurchaseDate,
                UserName = ticket.User!.Username,
                RouteOrigin = ticket.Schedule!.Route!.Origin,
                RouteDestination = ticket.Schedule!.Route!.Destination,
                DepartureTime = ticket.Schedule!.DepartureTime
            };
        }

        public async Task<TicketDto> CreateTicketAsync(CreateTicketDto createTicketDto)
        {
            // Verificar si el asiento está disponible
            if (!await _scheduleService.IsSeatAvailableAsync(createTicketDto.ScheduleId, createTicketDto.SeatNumber))
            {
                throw new InvalidOperationException("El asiento no está disponible");
            }

            // Obtener el horario para calcular el precio
            var schedule = await _context.Schedules
                .Include(s => s.Route)
                .FirstOrDefaultAsync(s => s.Id == createTicketDto.ScheduleId);

            if (schedule == null)
            {
                throw new InvalidOperationException("El horario no existe");
            }

            var ticket = new Ticket
            {
                TicketNumber = await GenerateTicketNumberAsync(),
                ScheduleId = createTicketDto.ScheduleId,
                UserId = createTicketDto.UserId,
                SeatNumber = createTicketDto.SeatNumber,
                Price = schedule.Route!.BasePrice,
                Status = "Active",
                PurchaseDate = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            };

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            return await GetTicketByIdAsync(ticket.Id) 
                ?? throw new InvalidOperationException("Failed to create ticket");
        }

        public async Task<TicketDto?> UpdateTicketAsync(int id, UpdateTicketDto updateTicketDto)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null) return null;

            if (updateTicketDto.Status != null) ticket.Status = updateTicketDto.Status;

            ticket.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return await GetTicketByIdAsync(id);
        }

        public async Task<bool> DeleteTicketAsync(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null) return false;

            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateTicketStatusAsync(int id, string status)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null) return false;

            ticket.Status = status;
            ticket.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<TicketDto>> GetTicketsByUserAsync(int userId)
        {
            return await _context.Tickets
                .Include(t => t.Schedule)
                    .ThenInclude(s => s!.Route)
                .Include(t => t.Schedule)
                    .ThenInclude(s => s!.Bus)
                .Include(t => t.User)
                .Where(t => t.UserId == userId)
                .Select(t => new TicketDto
                {
                    Id = t.Id,
                    TicketNumber = t.TicketNumber,
                    ScheduleId = t.ScheduleId,
                    UserId = t.UserId,
                    SeatNumber = t.SeatNumber,
                    Price = t.Price,
                    Status = t.Status,
                    PurchaseDate = t.PurchaseDate,
                    UserName = t.User!.Username,
                    RouteOrigin = t.Schedule!.Route!.Origin,
                    RouteDestination = t.Schedule!.Route!.Destination,
                    DepartureTime = t.Schedule!.DepartureTime
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<TicketDto>> GetTicketsByScheduleAsync(int scheduleId)
        {
            return await _context.Tickets
                .Include(t => t.Schedule)
                    .ThenInclude(s => s!.Route)
                .Include(t => t.Schedule)
                    .ThenInclude(s => s!.Bus)
                .Include(t => t.User)
                .Where(t => t.ScheduleId == scheduleId)
                .Select(t => new TicketDto
                {
                    Id = t.Id,
                    TicketNumber = t.TicketNumber,
                    ScheduleId = t.ScheduleId,
                    UserId = t.UserId,
                    SeatNumber = t.SeatNumber,
                    Price = t.Price,
                    Status = t.Status,
                    PurchaseDate = t.PurchaseDate,
                    UserName = t.User!.Username,
                    RouteOrigin = t.Schedule!.Route!.Origin,
                    RouteDestination = t.Schedule!.Route!.Destination,
                    DepartureTime = t.Schedule!.DepartureTime
                })
                .ToListAsync();
        }

        public async Task<string> GenerateTicketNumberAsync()
        {
            var lastTicket = await _context.Tickets
                .OrderByDescending(t => t.Id)
                .FirstOrDefaultAsync();

            var nextNumber = (lastTicket?.Id ?? 0) + 1;
            return $"TKT-{DateTime.UtcNow:yyyyMMdd}-{nextNumber:D6}";
        }
    }
} 