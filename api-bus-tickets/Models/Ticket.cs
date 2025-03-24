using System;

namespace api_bus_tickets.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public required string TicketNumber { get; set; }
        public int ScheduleId { get; set; }
        public int UserId { get; set; }
        public int SeatNumber { get; set; }
        public decimal Price { get; set; }
        public required string Status { get; set; } // Active, Cancelled, Used
        public DateTime PurchaseDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navegación
        public Schedule? Schedule { get; set; }
        public User? User { get; set; }
    }
} 