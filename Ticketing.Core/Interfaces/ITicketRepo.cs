using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticketing.Data.Models;

namespace Ticketing.Core.Interfaces
{
    public interface ITicketRepo
    {
        Task<TicketType> GetTicketType(string T_type);
        Task<Ticket> GetById(int id);
        Task<Ticket> GetByTicketIdentifier(int id);
        Task<DataTable> ExecuteTicketSP(int id = default, DateTime? activeDate = null, string address = null, decimal? latitude = null,
                                        decimal? longitude = null, string comment = null, bool? checkIn = null, int? statusId = null,
                                        int? ticketTypeId = null, string? filterString = null, string? columnList = null);
        Task<int> GetTicketsCountByOperatorId(int id, DateTime date);
    }
}
