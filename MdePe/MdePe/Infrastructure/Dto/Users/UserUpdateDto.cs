using System;
using System.Collections.Generic;
using System.Text;

namespace MdePe.Infrastructure.Dto.Users
{
    public class UserUpdateDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public int Length { get; set; }
        public DateTime Dob { get; set; }
        public double Weight { get; set; }
    }
}
