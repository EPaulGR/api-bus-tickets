namespace api_bus_tickets.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "USD";
        public string Status { get; set; } = "Pending"; // Pending, Completed, Failed, Refunded
        public string StripePaymentIntentId { get; set; } = string.Empty;
        public string StripeCustomerId { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navegación
        public Ticket? Ticket { get; set; }
        public User? User { get; set; }
    }
} 