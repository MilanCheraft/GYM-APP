using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MdePe.Infrastructure.Dto.Users
{
    public class UserDto
    {
        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("birthDate")]
        public DateTime BirthDate { get; set; }

        [JsonProperty("weight")]
        public int Weight { get; set; }

        [JsonProperty("length")]
        public int Length { get; set; }

    }
}

