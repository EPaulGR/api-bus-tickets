using System;

namespace api_bus_tickets.DTOs
{
    public class BusDto
    {
        public int Id { get; set; }
        public string PlateNumber { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int TotalSeats { get; set; }
        public string Type { get; set; } = string.Empty;
        public bool HasWifi { get; set; }
        public bool HasAirConditioning { get; set; }
        public bool HasBathroom { get; set; }
        public bool IsActive { get; set; }
        public DateTime LastMaintenanceDate { get; set; }
    }

    public class CreateBusDto
    {
        public string PlateNumber { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int TotalSeats { get; set; }
        public string Type { get; set; } = string.Empty;
        public bool HasWifi { get; set; }
        public bool HasAirConditioning { get; set; }
        public bool HasBathroom { get; set; }
    }

    public class UpdateBusDto
    {
        public string? PlateNumber { get; set; }
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public int? TotalSeats { get; set; }
        public string? Type { get; set; }
        public bool? HasWifi { get; set; }
        public bool? HasAirConditioning { get; set; }
        public bool? HasBathroom { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? LastMaintenanceDate { get; set; }
    }
} 