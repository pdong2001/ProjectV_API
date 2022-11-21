using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts.Users
{
    public class CreateUpdateUserDto
    {
        [Required]
        [MinLength(5)]
        public string UserName { get; set; }
        [Required, MinLength(8)]
        public string Password { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string[]? Roles { get; set; }
    }
}
