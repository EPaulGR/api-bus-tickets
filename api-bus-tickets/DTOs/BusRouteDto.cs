using System;

namespace api_bus_tickets.DTOs
{
    public class BusRouteDto
    {
        public int Id { get; set; }
        public string Origin { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
        public decimal Distance { get; set; }
        public TimeSpan EstimatedDuration { get; set; }
        public decimal BasePrice { get; set; }
        public bool IsActive { get; set; }
    }

    public class CreateBusRouteDto
    {
        public string Origin { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
        public decimal Distance { get; set; }
        public TimeSpan EstimatedDuration { get; set; }
        public decimal BasePrice { get; set; }
    }

    public class UpdateBusRouteDto
    {
        public string? Origin { get; set; }
        public string? Destination { get; set; }
        public decimal? Distance { get; set; }
        public TimeSpan? EstimatedDuration { get; set; }
        public decimal? BasePrice { get; set; }
        public bool? IsActive { get; set; }
    }
} 