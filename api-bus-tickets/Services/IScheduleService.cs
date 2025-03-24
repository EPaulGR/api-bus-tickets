using api_bus_tickets.DTOs;

namespace api_bus_tickets.Services
{
    public interface IScheduleService
    {
        Task<IEnumerable<ScheduleDto>> GetAllSchedulesAsync();
        Task<ScheduleDto?> GetScheduleByIdAsync(int id);
        Task<ScheduleDto> CreateScheduleAsync(CreateScheduleDto createScheduleDto);
        Task<ScheduleDto?> UpdateScheduleAsync(int id, UpdateScheduleDto updateScheduleDto);
        Task<bool> DeleteScheduleAsync(int id);
        Task<IEnumerable<ScheduleDto>> GetSchedulesByDateAsync(DateTime date);
        Task<IEnumerable<ScheduleDto>> GetSchedulesByRouteAsync(int routeId);
        Task<bool> IsSeatAvailableAsync(int scheduleId, int seatNumber);
    }
} 