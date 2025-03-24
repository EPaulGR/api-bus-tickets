using api_bus_tickets.Data;
using api_bus_tickets.DTOs;
using api_bus_tickets.Models;
using Microsoft.EntityFrameworkCore;

namespace api_bus_tickets.Services
{
    public class BusService : IBusService
    {
        private readonly ApplicationDbContext _context;

        public BusService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BusDto>> GetAllBusesAsync()
        {
            return await _context.Buses
                .Select(b => new BusDto
                {
                    Id = b.Id,
                    PlateNumber = b.PlateNumber,
                    Brand = b.Brand,
                    Model = b.Model,
                    TotalSeats = b.TotalSeats,
                    Type = b.Type,
                    HasWifi = b.HasWifi,
                    HasAirConditioning = b.HasAirConditioning,
                    HasBathroom = b.HasBathroom,
                    IsActive = b.IsActive,
                    LastMaintenanceDate = b.LastMaintenanceDate
                })
                .ToListAsync();
        }

        public async Task<BusDto?> GetBusByIdAsync(int id)
        {
            var bus = await _context.Buses.FindAsync(id);
            if (bus == null) return null;

            return new BusDto
            {
                Id = bus.Id,
                PlateNumber = bus.PlateNumber,
                Brand = bus.Brand,
                Model = bus.Model,
                TotalSeats = bus.TotalSeats,
                Type = bus.Type,
                HasWifi = bus.HasWifi,
                HasAirConditioning = bus.HasAirConditioning,
                HasBathroom = bus.HasBathroom,
                IsActive = bus.IsActive,
                LastMaintenanceDate = bus.LastMaintenanceDate
            };
        }

        public async Task<BusDto> CreateBusAsync(CreateBusDto createBusDto)
        {
            var bus = new Bus
            {
                PlateNumber = createBusDto.PlateNumber,
                Brand = createBusDto.Brand,
                Model = createBusDto.Model,
                TotalSeats = createBusDto.TotalSeats,
                Type = createBusDto.Type,
                HasWifi = createBusDto.HasWifi,
                HasAirConditioning = createBusDto.HasAirConditioning,
                HasBathroom = createBusDto.HasBathroom,
                IsActive = true,
                LastMaintenanceDate = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            };

            _context.Buses.Add(bus);
            await _context.SaveChangesAsync();

            return new BusDto
            {
                Id = bus.Id,
                PlateNumber = bus.PlateNumber,
                Brand = bus.Brand,
                Model = bus.Model,
                TotalSeats = bus.TotalSeats,
                Type = bus.Type,
                HasWifi = bus.HasWifi,
                HasAirConditioning = bus.HasAirConditioning,
                HasBathroom = bus.HasBathroom,
                IsActive = bus.IsActive,
                LastMaintenanceDate = bus.LastMaintenanceDate
            };
        }

        public async Task<BusDto?> UpdateBusAsync(int id, UpdateBusDto updateBusDto)
        {
            var bus = await _context.Buses.FindAsync(id);
            if (bus == null) return null;

            if (updateBusDto.PlateNumber != null) bus.PlateNumber = updateBusDto.PlateNumber;
            if (updateBusDto.Brand != null) bus.Brand = updateBusDto.Brand;
            if (updateBusDto.Model != null) bus.Model = updateBusDto.Model;
            if (updateBusDto.TotalSeats.HasValue) bus.TotalSeats = updateBusDto.TotalSeats.Value;
            if (updateBusDto.Type != null) bus.Type = updateBusDto.Type;
            if (updateBusDto.HasWifi.HasValue) bus.HasWifi = updateBusDto.HasWifi.Value;
            if (updateBusDto.HasAirConditioning.HasValue) bus.HasAirConditioning = updateBusDto.HasAirConditioning.Value;
            if (updateBusDto.HasBathroom.HasValue) bus.HasBathroom = updateBusDto.HasBathroom.Value;
            if (updateBusDto.IsActive.HasValue) bus.IsActive = updateBusDto.IsActive.Value;
            if (updateBusDto.LastMaintenanceDate.HasValue) bus.LastMaintenanceDate = updateBusDto.LastMaintenanceDate.Value;

            bus.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return new BusDto
            {
                Id = bus.Id,
                PlateNumber = bus.PlateNumber,
                Brand = bus.Brand,
                Model = bus.Model,
                TotalSeats = bus.TotalSeats,
                Type = bus.Type,
                HasWifi = bus.HasWifi,
                HasAirConditioning = bus.HasAirConditioning,
                HasBathroom = bus.HasBathroom,
                IsActive = bus.IsActive,
                LastMaintenanceDate = bus.LastMaintenanceDate
            };
        }

        public async Task<bool> DeleteBusAsync(int id)
        {
            var bus = await _context.Buses.FindAsync(id);
            if (bus == null) return false;

            _context.Buses.Remove(bus);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IsBusAvailableAsync(int id, DateTime date)
        {
            var bus = await _context.Buses
                .Include(b => b.Schedules)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (bus == null) return false;

            // Verificar si el bus está activo y no está en mantenimiento
            if (!bus.IsActive) return false;

            // Verificar si hay horarios programados para la fecha
            var hasSchedules = bus.Schedules.Any(s => 
                s.IsActive && 
                s.DepartureTime.Date == date.Date);

            return !hasSchedules;
        }
    }
} 