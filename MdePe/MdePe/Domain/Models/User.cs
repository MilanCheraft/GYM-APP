using System;
using System.Collections.Generic;
using System.Text;

namespace MdePe.Domain.Models
{
    public class User
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Length { get; set; }
        public DateTime DateOfBirth { get; set; }
        public double Weight { get; set; }

    }
}
