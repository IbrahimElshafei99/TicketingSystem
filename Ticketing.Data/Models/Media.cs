using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticketing.Data.Models
{
    public class Media
    {
        public int Id { get; set; }
        public string? BasicMeterImgPath { get; set; }
        public string? NewMeterImgPath { get; set; }
        public int? TicketId { get; set; }
        public Ticket? Ticket { get; set; }
        public int? UserId { get; set; }
        public User? User { get; set; }
    }
}
