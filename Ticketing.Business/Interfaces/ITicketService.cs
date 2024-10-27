using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticketing.Data.Models;

namespace Ticketing.Business.Interfaces
{
    public interface ITicketService
    {
        Task<IEnumerable<Ticket>> GetAllTickets();
    }
}
