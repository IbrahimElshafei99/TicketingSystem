using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticketing.Data.Models
{
    public class Ticket
    {
        [Required]
        public int TicketIdentifier { get; set; }
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
        //public ICollection<User_Ticket>? User_Tickets { get; set; }
    }
}
