using api_bus_tickets.DTOs;
using api_bus_tickets.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api_bus_tickets.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BusRouteController : ControllerBase
    {
        private readonly IBusRouteService _routeService;

        public BusRouteController(IBusRouteService routeService)
        {
            _routeService = routeService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BusRouteDto>>> GetAllRoutes()
        {
            var routes = await _routeService.GetAllRoutesAsync();
            return Ok(routes);
        }

        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<BusRouteDto>>> GetActiveRoutes()
        {
            var routes = await _routeService.GetActiveRoutesAsync();
            return Ok(routes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BusRouteDto>> GetRouteById(int id)
        {
            var route = await _routeService.GetRouteByIdAsync(id);
            if (route == null)
            {
                return NotFound();
            }
            return Ok(route);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<BusRouteDto>> CreateRoute(CreateBusRouteDto createRouteDto)
        {
            var route = await _routeService.CreateRouteAsync(createRouteDto);
            return CreatedAtAction(nameof(GetRouteById), new { id = route.Id }, route);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<BusRouteDto>> UpdateRoute(int id, UpdateBusRouteDto updateRouteDto)
        {
            var route = await _routeService.UpdateRouteAsync(id, updateRouteDto);
            if (route == null)
            {
                return NotFound();
            }
            return Ok(route);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteRoute(int id)
        {
            var result = await _routeService.DeleteRouteAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
} 