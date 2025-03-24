using System;

namespace api_bus_tickets.Models
{
    public class BusRoute
    {
        public int Id { get; set; }
        public required string Origin { get; set; }
        public required string Destination { get; set; }
        public decimal Distance { get; set; } // en kilómetros
        public TimeSpan EstimatedDuration { get; set; }
        public decimal BasePrice { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
} 