using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MdePe.Infrastructure.Dto.Users
{
    public class BaseUserdto
    {
        [JsonProperty("userId")]
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
