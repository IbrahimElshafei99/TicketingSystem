using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticketing.Core.DTO
{
    public class UserLoginDTO
    {
        [Required]
        public string? PhoneNumber { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
