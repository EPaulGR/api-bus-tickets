using api_bus_tickets.DTOs;

namespace api_bus_tickets.Services
{
    public interface ITicketService
    {
        Task<IEnumerable<TicketDto>> GetAllTicketsAsync();
        Task<TicketDto?> GetTicketByIdAsync(int id);
        Task<IEnumerable<TicketDto>> GetTicketsByUserAsync(int userId);
        Task<IEnumerable<TicketDto>> GetTicketsByScheduleAsync(int scheduleId);
        Task<TicketDto> CreateTicketAsync(CreateTicketDto createTicketDto);
        Task<TicketDto?> UpdateTicketAsync(int id, UpdateTicketDto updateTicketDto);
        Task<bool> DeleteTicketAsync(int id);
        Task<bool> UpdateTicketStatusAsync(int id, string status);
        Task<string> GenerateTicketNumberAsync();
    }
} 