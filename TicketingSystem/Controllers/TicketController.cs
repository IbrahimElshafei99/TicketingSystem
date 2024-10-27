using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ticketing.Business.Interfaces;

namespace TicketingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [Authorize]
        [HttpGet]
        [Route("GetTickets")]
        public async Task<IActionResult> GetTickets()
        {
            var tickets = await _ticketService.GetAllTickets();
            if (tickets == null)
            {
                return NotFound();
            }
            return Ok(tickets);
        }
    }
}
