using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts.Users
{
    public class UserLoginResponseDto
    {
        public UserDto User { get; set; }
        public string Token { get; set; }
        public DateTime Expires { get; set; }
    }
}
