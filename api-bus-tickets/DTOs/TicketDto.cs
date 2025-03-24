using System;

namespace api_bus_tickets.DTOs
{
    public class TicketDto
    {
        public int Id { get; set; }
        public string TicketNumber { get; set; } = string.Empty;
        public int ScheduleId { get; set; }
        public int UserId { get; set; }
        public int SeatNumber { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime PurchaseDate { get; set; }
        public string? UserName { get; set; }
        public string? RouteOrigin { get; set; }
        public string? RouteDestination { get; set; }
        public DateTime? DepartureTime { get; set; }
    }

    public class CreateTicketDto
    {
        public int ScheduleId { get; set; }
        public int UserId { get; set; }
        public int SeatNumber { get; set; }
    }

    public class UpdateTicketDto
    {
        public string? Status { get; set; }
    }
} 