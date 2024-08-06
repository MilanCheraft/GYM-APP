using MdePe.Domain.Models;
using MdePe.Infrastructure.Dto.Gym;
using MdePe.Infrastructure.Dto.MuscleGroups;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MdePe.Domain.Services.Api
{
    public class ApiGymsService : IGymService
    {
        private string _jsonUrl = ApiConstants.GymJsonUrl;
        public async Task<IEnumerable<Gym>> GetGymAsync()
        {
            using (var client = new HttpClient())
            {
                var result = await client.GetAsync(_jsonUrl);
                if (result.IsSuccessStatusCode)
                {
                    //get and parse json
                    string jsonContent = await result.Content.ReadAsStringAsync();
                    List<GymDto> gymDto = JsonConvert.DeserializeObject<List<GymDto>>(jsonContent);
                    var gyms = gymDto.Select(g => new Gym
                    {
                        Id = g.Id,
                        Name = g.Name,
                        Address = g.Address,
                        latitude = g.latitude,
                        longitude = g.longitude,
                    }).ToList();
                    return gyms;
                }
                return null;
            }
        }
    }
}
