using api_bus_tickets.DTOs;
using api_bus_tickets.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace api_bus_tickets.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<TicketDto>>> GetAllTickets()
        {
            var tickets = await _ticketService.GetAllTicketsAsync();
            return Ok(tickets);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TicketDto>> GetTicketById(int id)
        {
            var ticket = await _ticketService.GetTicketByIdAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }

            // Solo permitir que los usuarios vean sus propios tickets o los administradores vean cualquier ticket
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            if (ticket.UserId != currentUserId && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            return Ok(ticket);
        }

        [HttpGet("user/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<TicketDto>>> GetTicketsByUser(int userId)
        {
            var tickets = await _ticketService.GetTicketsByUserAsync(userId);
            return Ok(tickets);
        }

        [HttpGet("schedule/{scheduleId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<TicketDto>>> GetTicketsBySchedule(int scheduleId)
        {
            var tickets = await _ticketService.GetTicketsByScheduleAsync(scheduleId);
            return Ok(tickets);
        }

        [HttpPost]
        public async Task<ActionResult<TicketDto>> CreateTicket(CreateTicketDto createTicketDto)
        {
            // Asignar el ID del usuario actual al ticket
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            createTicketDto.UserId = currentUserId;

            var ticket = await _ticketService.CreateTicketAsync(createTicketDto);
            return CreatedAtAction(nameof(GetTicketById), new { id = ticket.Id }, ticket);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<TicketDto>> UpdateTicket(int id, UpdateTicketDto updateTicketDto)
        {
            var ticket = await _ticketService.UpdateTicketAsync(id, updateTicketDto);
            if (ticket == null)
            {
                return NotFound();
            }
            return Ok(ticket);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteTicket(int id)
        {
            var result = await _ticketService.DeleteTicketAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
} 