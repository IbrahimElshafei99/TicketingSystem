using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticketing.Data.Models
{
    public class User_Ticket
    {
        public int TicketId { get; set; }
        public Ticket? Ticket { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public int RoleId { get; set; }
        public Role? Role { get; set; }

    }
}
