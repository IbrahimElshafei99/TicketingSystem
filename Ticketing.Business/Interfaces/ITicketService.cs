using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticketing.Core.DTO;
using Ticketing.Data.Models;

namespace Ticketing.Business.Interfaces
{
    public interface ITicketService
    {
        string ConvertDataTableToJson(DataTable dataTable);
        Task<DataTable> GetAllTickets();
        Task OpenTicket(TicketForSTeamDTO ticketDto);
        Task<DataTable> ModifyTicketByOperator(TicketForOperatorDTO ticket);
        Task<Ticket> GetById(int id);
    }
}
