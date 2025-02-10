using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticketing.Data.Models;

namespace Ticketing.Core.DTO
{
    public class TicketForSTeamDTO
    {
        // business ticket identifier property => ticketCode
        public DateTime ActiveDate { get; set; }
        [Required]
        public string Address { get; set; } = string.Empty;
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; } 
        public string? Comment { get; set; }
        public string? TicketTypeText { get; set; }
    }
}
