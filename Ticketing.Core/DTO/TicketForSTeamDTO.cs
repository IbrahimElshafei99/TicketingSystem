using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticketing.Data.Models;

namespace Ticketing.Core.DTO
{
    public class TicketForSTeamDTO
    {
        public DateTime ActiveDate { get; set; }
        public string? Address { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string? Comment { get; set; }
        public int? TicketTypeId { get; set; }
    }
}
