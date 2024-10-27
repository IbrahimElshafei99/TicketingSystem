using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticketing.Core.DTO
{
    public class UserRegistrationDTO
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmedPassword { get; set; }
        public string? Email { get; set; }
        [Required]
        public string NationalId { get; set; }
        [Required]
        public string Address { get; set; }
    }
}
