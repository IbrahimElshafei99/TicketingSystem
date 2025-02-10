using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using Ticketing.Business.Interfaces;
using Ticketing.Core.DTO;
using Ticketing.Core.Interfaces;
using Ticketing.Data.Models;

namespace TicketingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;
        private readonly ILogger<TicketController> _logger;

        public TicketController(ITicketService ticketService, ILogger<TicketController> logger)
        {
            _ticketService = ticketService;
            _logger = logger;
        }

        //[Authorize]
        /*[HttpGet]
        [Route("GetTickets")]
        public async Task<IActionResult> GetTickets()
        {
            var tickets = await _ticketService.GetAllTickets();
            _logger.LogInformation("Get Request {@Ticket}", tickets);
            if (tickets == null)
            {
                return NotFound();
            }
            return Ok(tickets);
        }*/

        [HttpGet]
        [Route("AllTickets")]
        public async Task<IActionResult> GetAllticketsFromDT()
        {
            var DT = await _ticketService.GetAllTickets();
            var result = _ticketService.ConvertDataTableToJson(DT);
            return Ok(result);
        }

        [HttpGet]
        [Route("GetById")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                _logger.LogInformation("Successful log");
                _logger.LogInformation("Another log for method {method}", MethodBase.GetCurrentMethod().DeclaringType);

                var ticket = await _ticketService.GetById(id);
                if (ticket != null)
                {
                    return Ok(ticket);
                }
                throw new NullReferenceException("Ticket not found");

            }
            catch (Exception ex)
            {
                _logger.LogError("Ticket Not Found");
                return BadRequest(ex.Message);
            }
            
        }

        
        [HttpPost]
        [Route("OpenNewTicket")]
        public async Task<IActionResult> OpenNewTicket(TicketForSTeamDTO ticket)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // call create ticket method from Ticket services
                    await _ticketService.OpenTicket(ticket);
                    return Ok();
                }
                else
                {
                    throw new ArgumentException("Data invalid");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in openning new ticket: {message}",ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("ModifyByOperator")]
        public async Task<IActionResult> ModifyByOperator(TicketForOperatorDTO ticket)
        {
            var DT = await _ticketService.ModifyTicketByOperator(ticket);
            if (DT != null)
            {
                var result = _ticketService.ConvertDataTableToJson(DT);
                _logger.LogInformation("Successful modifing ticket: {result}",result);
                return Ok(result);
            }
            _logger.LogError("Error in modifing ticket");
            return BadRequest("Error in modifing ticket");

        }

        // API to change the status to closed or refused

        /* 
          "formatter": "Serilog.Formatting.Compact.CompactJsonFormatter, Serilog.Formatting.Compact",
          ,
          "outputTemplate": "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:j}{NewLine}{Properties:j}{NewLine}{Exception}"       
        */
    }
}
