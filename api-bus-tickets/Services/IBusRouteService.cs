using api_bus_tickets.DTOs;

namespace api_bus_tickets.Services
{
    public interface IBusRouteService
    {
        Task<IEnumerable<BusRouteDto>> GetAllRoutesAsync();
        Task<BusRouteDto?> GetRouteByIdAsync(int id);
        Task<BusRouteDto> CreateRouteAsync(CreateBusRouteDto createRouteDto);
        Task<BusRouteDto?> UpdateRouteAsync(int id, UpdateBusRouteDto updateRouteDto);
        Task<bool> DeleteRouteAsync(int id);
        Task<IEnumerable<BusRouteDto>> GetActiveRoutesAsync();
    }
} 