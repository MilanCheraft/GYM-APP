using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MdePe.Infrastructure.Dto.Gym
{
    public class GymDto
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }
        [JsonProperty("latitude")]
        public double latitude { get; set; }
        [JsonProperty("longitude")]
        public double longitude { get; set; }
    }
}
