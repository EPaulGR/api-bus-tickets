using api_bus_tickets.DTOs;
using api_bus_tickets.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api_bus_tickets.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BusController : ControllerBase
    {
        private readonly IBusService _busService;

        public BusController(IBusService busService)
        {
            _busService = busService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<BusDto>>> GetAllBuses()
        {
            var buses = await _busService.GetAllBusesAsync();
            return Ok(buses);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<BusDto>> GetBusById(int id)
        {
            var bus = await _busService.GetBusByIdAsync(id);
            if (bus == null)
            {
                return NotFound();
            }
            return Ok(bus);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<BusDto>> CreateBus(CreateBusDto createBusDto)
        {
            var bus = await _busService.CreateBusAsync(createBusDto);
            return CreatedAtAction(nameof(GetBusById), new { id = bus.Id }, bus);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<BusDto>> UpdateBus(int id, UpdateBusDto updateBusDto)
        {
            var bus = await _busService.UpdateBusAsync(id, updateBusDto);
            if (bus == null)
            {
                return NotFound();
            }
            return Ok(bus);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteBus(int id)
        {
            var result = await _busService.DeleteBusAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpGet("{id}/availability")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<bool>> CheckBusAvailability(int id, [FromQuery] DateTime date)
        {
            var isAvailable = await _busService.IsBusAvailableAsync(id, date);
            return Ok(isAvailable);
        }
    }
} 