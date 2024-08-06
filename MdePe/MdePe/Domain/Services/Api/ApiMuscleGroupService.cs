using MdePe.Domain.Models;
using MdePe.Infrastructure.Dto.Exercises;
using MdePe.Infrastructure.Dto.MuscleGroups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace MdePe.Domain.Services.Api
{
    public class ApiMuscleGroupService : IMuscleGroupService
    {
        private string baseUri = ApiConstants.BaseUrl;
        private readonly ISecureTokenService _secureTokenService;

        public ApiMuscleGroupService(ISecureTokenService secureTokenService)
        {
            _secureTokenService = secureTokenService;
        }

        public async Task<MuscleGroup> GetMuscleGroupById(int id)
        {
            using (var client = new HttpClient())
            {
                var token = await _secureTokenService.GetTokenAsync();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var result = await client.GetFromJsonAsync<MuscleGroupDto>($"{baseUri}/MuscleGroups/{id}");
                if(result == null)
                {
                    return null;
                }
                var muscleGroup = new MuscleGroup
                {
                    Id = result.Id,
                    Name = result.Name,
                };
                return muscleGroup;
            }
        }

        public async Task<List<MuscleGroup>> GetMuscleGroups()
        {
            using (var client = new HttpClient())
            {
                var token = await _secureTokenService.GetTokenAsync();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var result = await client.GetFromJsonAsync<MuscleGroupsDto>($"{baseUri}/MuscleGroups");
                if(result.MuscleGroups.Count() > 0)
                {
                    var muscleGroups = result.MuscleGroups.Select(mg => new MuscleGroup
                    {
                        Id = mg.Id,
                        Name = mg.Name,
                    }).ToList();
                    return muscleGroups;
                }
                return null;
            }
        }
    }
}
