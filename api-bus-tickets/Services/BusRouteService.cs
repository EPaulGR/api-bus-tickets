using api_bus_tickets.Data;
using api_bus_tickets.DTOs;
using api_bus_tickets.Models;
using Microsoft.EntityFrameworkCore;

namespace api_bus_tickets.Services
{
    public class BusRouteService : IBusRouteService
    {
        private readonly ApplicationDbContext _context;

        public BusRouteService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BusRouteDto>> GetAllRoutesAsync()
        {
            return await _context.Routes
                .Select(r => new BusRouteDto
                {
                    Id = r.Id,
                    Origin = r.Origin,
                    Destination = r.Destination,
                    Distance = r.Distance,
                    EstimatedDuration = r.EstimatedDuration,
                    BasePrice = r.BasePrice,
                    IsActive = r.IsActive
                })
                .ToListAsync();
        }

        public async Task<BusRouteDto?> GetRouteByIdAsync(int id)
        {
            var route = await _context.Routes.FindAsync(id);
            if (route == null) return null;

            return new BusRouteDto
            {
                Id = route.Id,
                Origin = route.Origin,
                Destination = route.Destination,
                Distance = route.Distance,
                EstimatedDuration = route.EstimatedDuration,
                BasePrice = route.BasePrice,
                IsActive = route.IsActive
            };
        }

        public async Task<BusRouteDto> CreateRouteAsync(CreateBusRouteDto createRouteDto)
        {
            var route = new BusRoute
            {
                Origin = createRouteDto.Origin,
                Destination = createRouteDto.Destination,
                Distance = createRouteDto.Distance,
                EstimatedDuration = createRouteDto.EstimatedDuration,
                BasePrice = createRouteDto.BasePrice,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.Routes.Add(route);
            await _context.SaveChangesAsync();

            return new BusRouteDto
            {
                Id = route.Id,
                Origin = route.Origin,
                Destination = route.Destination,
                Distance = route.Distance,
                EstimatedDuration = route.EstimatedDuration,
                BasePrice = route.BasePrice,
                IsActive = route.IsActive
            };
        }

        public async Task<BusRouteDto?> UpdateRouteAsync(int id, UpdateBusRouteDto updateRouteDto)
        {
            var route = await _context.Routes.FindAsync(id);
            if (route == null) return null;

            if (updateRouteDto.Origin != null) route.Origin = updateRouteDto.Origin;
            if (updateRouteDto.Destination != null) route.Destination = updateRouteDto.Destination;
            if (updateRouteDto.Distance.HasValue) route.Distance = updateRouteDto.Distance.Value;
            if (updateRouteDto.EstimatedDuration.HasValue) route.EstimatedDuration = updateRouteDto.EstimatedDuration.Value;
            if (updateRouteDto.BasePrice.HasValue) route.BasePrice = updateRouteDto.BasePrice.Value;
            if (updateRouteDto.IsActive.HasValue) route.IsActive = updateRouteDto.IsActive.Value;

            route.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return new BusRouteDto
            {
                Id = route.Id,
                Origin = route.Origin,
                Destination = route.Destination,
                Distance = route.Distance,
                EstimatedDuration = route.EstimatedDuration,
                BasePrice = route.BasePrice,
                IsActive = route.IsActive
            };
        }

        public async Task<bool> DeleteRouteAsync(int id)
        {
            var route = await _context.Routes.FindAsync(id);
            if (route == null) return false;

            _context.Routes.Remove(route);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<BusRouteDto>> GetActiveRoutesAsync()
        {
            return await _context.Routes
                .Where(r => r.IsActive)
                .Select(r => new BusRouteDto
                {
                    Id = r.Id,
                    Origin = r.Origin,
                    Destination = r.Destination,
                    Distance = r.Distance,
                    EstimatedDuration = r.EstimatedDuration,
                    BasePrice = r.BasePrice,
                    IsActive = r.IsActive
                })
                .ToListAsync();
        }
    }
} 