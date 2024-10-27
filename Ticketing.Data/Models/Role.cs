using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticketing.Data.Models
{
    public class Role
    {
        [Key]
        public int RoledId { get; set; }
        public string RoleName { get; set; }
        public int? StatusId { get; set; }
        public Status? Status { get; set; }
    }
}
