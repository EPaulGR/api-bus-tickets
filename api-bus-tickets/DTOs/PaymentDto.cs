namespace api_bus_tickets.DTOs
{
    public class PaymentDto
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string StripePaymentIntentId { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class CreatePaymentDto
    {
        public int TicketId { get; set; }
        public string PaymentMethodId { get; set; } = string.Empty;
        public string PaymentIntentId { get; set; } = string.Empty;
    }

    public class PaymentIntentDto
    {
        public string ClientSecret { get; set; } = string.Empty;
        public string PaymentIntentId { get; set; } = string.Empty;
    }
} 