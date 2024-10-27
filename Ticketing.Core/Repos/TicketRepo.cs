using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticketing.Core.Context;
using Ticketing.Core.Interfaces;
using Ticketing.Data.Models;

namespace Ticketing.Core.Repos
{
    public class TicketRepo : ITicketRepo
    {
        private readonly AppDbContext _context;

        public TicketRepo(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public async Task<IEnumerable<Ticket>> GetAllTickets()
        {
            return await _context.Ticket.ToListAsync();
        }
    }

}
