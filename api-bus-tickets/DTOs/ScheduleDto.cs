using System;

namespace api_bus_tickets.DTOs
{
    public class ScheduleDto
    {
        public int Id { get; set; }
        public int RouteId { get; set; }
        public int BusId { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public int AvailableSeats { get; set; }
        public bool IsActive { get; set; }
        public string? RouteOrigin { get; set; }
        public string? RouteDestination { get; set; }
        public string? BusPlateNumber { get; set; }
    }

    public class CreateScheduleDto
    {
        public int RouteId { get; set; }
        public int BusId { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
    }

    public class UpdateScheduleDto
    {
        public DateTime? DepartureTime { get; set; }
        public DateTime? ArrivalTime { get; set; }
        public int? AvailableSeats { get; set; }
        public bool? IsActive { get; set; }
    }
} 