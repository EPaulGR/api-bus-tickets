using System;

namespace api_bus_tickets.Models
{
    public class Schedule
    {
        public int Id { get; set; }
        public int RouteId { get; set; }
        public int BusId { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public int AvailableSeats { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navegación
        public BusRoute? Route { get; set; }
        public Bus? Bus { get; set; }
    }
} 