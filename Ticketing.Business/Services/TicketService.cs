using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticketing.Business.Interfaces;
using Ticketing.Core.Interfaces;
using Ticketing.Data.Models;

namespace Ticketing.Business.Services
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepo _ticketRepo;
        public TicketService(ITicketRepo ticketRepo)
        {
            _ticketRepo = ticketRepo;
        }

        public async Task<IEnumerable<Ticket>> GetAllTickets()
        {
            return await _ticketRepo.GetAllTickets();
        }
    }
}
