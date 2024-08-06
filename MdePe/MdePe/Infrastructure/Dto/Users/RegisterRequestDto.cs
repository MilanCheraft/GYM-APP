using System;
using System.Collections.Generic;
using System.Text;

namespace MdePe.Infrastructure.Dto.Users
{
    public class RegisterRequestDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string RepeatPassword { get; set; }
        public DateTime BirthDate { get; set; }
        public double Weight { get; set; }
        public int Length { get; set; }
        public string DisplayName { get; set; }
    }

}

