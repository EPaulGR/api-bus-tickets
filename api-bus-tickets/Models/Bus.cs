using System;

namespace api_bus_tickets.Models
{
    public class Bus
    {
        public int Id { get; set; }
        public required string PlateNumber { get; set; }
        public required string Brand { get; set; }
        public required string Model { get; set; }
        public int TotalSeats { get; set; }
        public required string Type { get; set; } // Regular, Premium, etc.
        public bool HasWifi { get; set; }
        public bool HasAirConditioning { get; set; }
        public bool HasBathroom { get; set; }
        public bool IsActive { get; set; }
        public DateTime LastMaintenanceDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navegación
        public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
    }
} 