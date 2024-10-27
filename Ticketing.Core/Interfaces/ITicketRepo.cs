using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticketing.Data.Models;

namespace Ticketing.Core.Interfaces
{
    public interface ITicketRepo
    {
        Task<IEnumerable<Ticket>> GetAllTickets();
    }
}
