using api_bus_tickets.DTOs;

namespace api_bus_tickets.Services
{
    public interface IBusService
    {
        Task<IEnumerable<BusDto>> GetAllBusesAsync();
        Task<BusDto?> GetBusByIdAsync(int id);
        Task<BusDto> CreateBusAsync(CreateBusDto createBusDto);
        Task<BusDto?> UpdateBusAsync(int id, UpdateBusDto updateBusDto);
        Task<bool> DeleteBusAsync(int id);
        Task<bool> IsBusAvailableAsync(int id, DateTime date);
    }
} 