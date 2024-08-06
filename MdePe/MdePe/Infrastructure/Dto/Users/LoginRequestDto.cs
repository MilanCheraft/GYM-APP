using System;
using System.Collections.Generic;
using System.Text;

namespace MdePe.Infrastructure.Dto.Users
{
    public class LoginRequestDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
