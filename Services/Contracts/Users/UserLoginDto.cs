using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts.Users
{
    public class UserLoginRequestDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        [DefaultValue(false)]
        public bool Remember { get; set; }
    }
}
