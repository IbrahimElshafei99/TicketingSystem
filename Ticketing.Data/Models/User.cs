using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticketing.Data.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        [Required]
        public string PhoneNumber {  get; set; }
        [Required]
        public string Social_Id { get; set; }
        [Required]
        public string Password { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public int StatusId { get; set; }
        public Status? Status { get; set; }
        public int RoleId { get; set; }
        public Role? Role { get; set; }



    }
}
