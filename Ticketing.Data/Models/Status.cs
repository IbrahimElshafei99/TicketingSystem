using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticketing.Data.Models
{
    public class Status
    {
        public int Id { get; set; }
        public string? Status_Text { get; set; }
        public string? EntityName { get; set; }
    }
}
