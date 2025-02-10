using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticketing.Data.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public DateTime? ActiveDate { get; set; }
        public string? Address { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string? Comment { get; set; }
        public bool? CheckIn { get; set; }

        public int? StatusId { get; set; }
        public Status? Status { get; set; }
        public int? TicketTypeId { get; set; }
        public TicketType? TicketType { get; set; }
    }
}
