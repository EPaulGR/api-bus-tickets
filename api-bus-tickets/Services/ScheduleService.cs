using api_bus_tickets.Data;
using api_bus_tickets.DTOs;
using api_bus_tickets.Models;
using Microsoft.EntityFrameworkCore;

namespace api_bus_tickets.Services
{
    public class ScheduleService : IScheduleService
    {
        private readonly ApplicationDbContext _context;

        public ScheduleService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ScheduleDto>> GetAllSchedulesAsync()
        {
            return await _context.Schedules
                .Include(s => s.Route)
                .Include(s => s.Bus)
                .Select(s => new ScheduleDto
                {
                    Id = s.Id,
                    RouteId = s.RouteId,
                    BusId = s.BusId,
                    DepartureTime = s.DepartureTime,
                    ArrivalTime = s.ArrivalTime,
                    AvailableSeats = s.AvailableSeats,
                    IsActive = s.IsActive,
                    RouteOrigin = s.Route!.Origin,
                    RouteDestination = s.Route!.Destination,
                    BusPlateNumber = s.Bus!.PlateNumber
                })
                .ToListAsync();
        }

        public async Task<ScheduleDto?> GetScheduleByIdAsync(int id)
        {
            var schedule = await _context.Schedules
                .Include(s => s.Route)
                .Include(s => s.Bus)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (schedule == null) return null;

            return new ScheduleDto
            {
                Id = schedule.Id,
                RouteId = schedule.RouteId,
                BusId = schedule.BusId,
                DepartureTime = schedule.DepartureTime,
                ArrivalTime = schedule.ArrivalTime,
                AvailableSeats = schedule.AvailableSeats,
                IsActive = schedule.IsActive,
                RouteOrigin = schedule.Route!.Origin,
                RouteDestination = schedule.Route!.Destination,
                BusPlateNumber = schedule.Bus!.PlateNumber
            };
        }

        public async Task<ScheduleDto> CreateScheduleAsync(CreateScheduleDto createScheduleDto)
        {
            var schedule = new Schedule
            {
                RouteId = createScheduleDto.RouteId,
                BusId = createScheduleDto.BusId,
                DepartureTime = createScheduleDto.DepartureTime,
                ArrivalTime = createScheduleDto.ArrivalTime,
                AvailableSeats = 0, // Se actualizará basado en el bus
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            // Obtener el bus para establecer los asientos disponibles
            var bus = await _context.Buses.FindAsync(createScheduleDto.BusId);
            if (bus != null)
            {
                schedule.AvailableSeats = bus.TotalSeats;
            }

            _context.Schedules.Add(schedule);
            await _context.SaveChangesAsync();

            return await GetScheduleByIdAsync(schedule.Id) 
                ?? throw new InvalidOperationException("Failed to create schedule");
        }

        public async Task<ScheduleDto?> UpdateScheduleAsync(int id, UpdateScheduleDto updateScheduleDto)
        {
            var schedule = await _context.Schedules.FindAsync(id);
            if (schedule == null) return null;

            if (updateScheduleDto.DepartureTime.HasValue) schedule.DepartureTime = updateScheduleDto.DepartureTime.Value;
            if (updateScheduleDto.ArrivalTime.HasValue) schedule.ArrivalTime = updateScheduleDto.ArrivalTime.Value;
            if (updateScheduleDto.AvailableSeats.HasValue) schedule.AvailableSeats = updateScheduleDto.AvailableSeats.Value;
            if (updateScheduleDto.IsActive.HasValue) schedule.IsActive = updateScheduleDto.IsActive.Value;

            schedule.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return await GetScheduleByIdAsync(id);
        }

        public async Task<bool> DeleteScheduleAsync(int id)
        {
            var schedule = await _context.Schedules.FindAsync(id);
            if (schedule == null) return false;

            _context.Schedules.Remove(schedule);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<ScheduleDto>> GetSchedulesByDateAsync(DateTime date)
        {
            return await _context.Schedules
                .Include(s => s.Route)
                .Include(s => s.Bus)
                .Where(s => s.DepartureTime.Date == date.Date && s.IsActive)
                .Select(s => new ScheduleDto
                {
                    Id = s.Id,
                    RouteId = s.RouteId,
                    BusId = s.BusId,
                    DepartureTime = s.DepartureTime,
                    ArrivalTime = s.ArrivalTime,
                    AvailableSeats = s.AvailableSeats,
                    IsActive = s.IsActive,
                    RouteOrigin = s.Route!.Origin,
                    RouteDestination = s.Route!.Destination,
                    BusPlateNumber = s.Bus!.PlateNumber
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<ScheduleDto>> GetSchedulesByRouteAsync(int routeId)
        {
            return await _context.Schedules
                .Include(s => s.Route)
                .Include(s => s.Bus)
                .Where(s => s.RouteId == routeId && s.IsActive)
                .Select(s => new ScheduleDto
                {
                    Id = s.Id,
                    RouteId = s.RouteId,
                    BusId = s.BusId,
                    DepartureTime = s.DepartureTime,
                    ArrivalTime = s.ArrivalTime,
                    AvailableSeats = s.AvailableSeats,
                    IsActive = s.IsActive,
                    RouteOrigin = s.Route!.Origin,
                    RouteDestination = s.Route!.Destination,
                    BusPlateNumber = s.Bus!.PlateNumber
                })
                .ToListAsync();
        }

        public async Task<bool> IsSeatAvailableAsync(int scheduleId, int seatNumber)
        {
            var schedule = await _context.Schedules
                .Include(s => s.Bus)
                .FirstOrDefaultAsync(s => s.Id == scheduleId);

            if (schedule == null || !schedule.IsActive) return false;

            // Verificar si el número de asiento está dentro del rango válido
            if (seatNumber < 1 || seatNumber > schedule.Bus!.TotalSeats) return false;

            // Verificar si el asiento ya está reservado
            var isSeatTaken = await _context.Tickets
                .AnyAsync(t => t.ScheduleId == scheduleId && 
                              t.SeatNumber == seatNumber && 
                              t.Status == "Active");

            return !isSeatTaken;
        }
    }
} 