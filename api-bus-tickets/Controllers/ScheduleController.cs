using api_bus_tickets.DTOs;
using api_bus_tickets.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api_bus_tickets.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleService _scheduleService;

        public ScheduleController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ScheduleDto>>> GetAllSchedules()
        {
            var schedules = await _scheduleService.GetAllSchedulesAsync();
            return Ok(schedules);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ScheduleDto>> GetScheduleById(int id)
        {
            var schedule = await _scheduleService.GetScheduleByIdAsync(id);
            if (schedule == null)
            {
                return NotFound();
            }
            return Ok(schedule);
        }

        [HttpGet("date/{date}")]
        public async Task<ActionResult<IEnumerable<ScheduleDto>>> GetSchedulesByDate(DateTime date)
        {
            var schedules = await _scheduleService.GetSchedulesByDateAsync(date);
            return Ok(schedules);
        }

        [HttpGet("route/{routeId}")]
        public async Task<ActionResult<IEnumerable<ScheduleDto>>> GetSchedulesByRoute(int routeId)
        {
            var schedules = await _scheduleService.GetSchedulesByRouteAsync(routeId);
            return Ok(schedules);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ScheduleDto>> CreateSchedule(CreateScheduleDto createScheduleDto)
        {
            var schedule = await _scheduleService.CreateScheduleAsync(createScheduleDto);
            return CreatedAtAction(nameof(GetScheduleById), new { id = schedule.Id }, schedule);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ScheduleDto>> UpdateSchedule(int id, UpdateScheduleDto updateScheduleDto)
        {
            var schedule = await _scheduleService.UpdateScheduleAsync(id, updateScheduleDto);
            if (schedule == null)
            {
                return NotFound();
            }
            return Ok(schedule);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteSchedule(int id)
        {
            var result = await _scheduleService.DeleteScheduleAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpGet("{id}/seat/{seatNumber}/availability")]
        public async Task<ActionResult<bool>> CheckSeatAvailability(int id, int seatNumber)
        {
            var isAvailable = await _scheduleService.IsSeatAvailableAsync(id, seatNumber);
            return Ok(isAvailable);
        }
    }
} 