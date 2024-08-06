using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace MdePe.Infrastructure.Dto.Users
{
    public class LoginDto
    {
        [JsonProperty("bearerToken")]
        public string BearerToken { get; set; }
    }
}
