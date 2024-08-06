using MdePe.Domain.Models;
using MdePe.Infrastructure.Dto.Exercises;
using MdePe.Infrastructure.Dto.Users;
using MdePe.Infrastructure.Results;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MdePe.Domain.Services.Api
{
    public class ApiExerciseService : IExerciseService
    {
        private string baseUri = ApiConstants.BaseUrl;
        private readonly IMuscleGroupService _muscleGroupService;
        private readonly ISecureTokenService _secureTokenService;

        public ApiExerciseService(IMuscleGroupService muscleGroupService, ISecureTokenService secureTokenService)
        {
            _muscleGroupService = muscleGroupService;
            _secureTokenService = secureTokenService;
        }

        public async Task<List<Exercise>> GetExercises()
        {
            using (var client = new HttpClient())
            {
                var token = await _secureTokenService.GetTokenAsync();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var result = await client.GetFromJsonAsync<ExercisesDto>($"{baseUri}/Exercises");
                if (result.Exercises.Count() < 0)
                {
                    return null;
                }
                List<Exercise> exercises = new List<Exercise>();
                foreach (var e in result.Exercises)
                {
                    var muscleGroup = await _muscleGroupService.GetMuscleGroupById(e.MuscleGroupId);
                    var exercise = new Exercise
                    {
                        Id = e.Id,
                        Name = e.Name,
                        Description = e.Description,
                        MuscleGroupName = muscleGroup,
                        Reps = e.Reps,
                        Sets = e.Sets,
                        Weight = e.Weight
                    };

                    exercises.Add(exercise);
                }

                return exercises;
            }
        }


        public async Task<Exercise> GetExercisesById(int id)
        {
            using (var client = new HttpClient())
            {
                var token = await _secureTokenService.GetTokenAsync();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var result = await client.GetFromJsonAsync<ExerciseDto>($"{baseUri}/Exercises/{id}");
                if (result == null)
                {
                    return null;
                }
                var exercise = new Exercise
                {
                    Id = result.Id,
                    Name = result.Name,
                    Description = result.Description,
                    MuscleGroupName = await _muscleGroupService.GetMuscleGroupById(result.MuscleGroupId),
                    Reps = result.Reps,
                    Sets = result.Sets,
                    Weight = result.Weight,
                };

                return exercise;
            }
        }

        public async Task<bool> UpdateExerciseAsync(Exercise exercise)
        {
            var updateData = new ExerciseUpdateRequestDto
            {
                Id = exercise.Id,
                Name = exercise.Name,
                Description = exercise.Description,
                MuscleGroupId = exercise.MuscleGroupName.Id,
                Reps = exercise.Reps,
                Sets = exercise.Sets,
                Weight = exercise.Weight
            };
            var content = new StringContent(JsonConvert.SerializeObject(updateData), Encoding.UTF8, "application/json");
            using (var client = new HttpClient())
            {
                var token = await _secureTokenService.GetTokenAsync();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var result = await client.PutAsync($"{baseUri}/Exercises", content);
                if (result.IsSuccessStatusCode)
                {
                    return true;
                }
                return false;
            }
        }

        public async Task<bool> DeleteExerciseAsync(int id)
        {
            using (var client = new HttpClient())
            {
                var token = await _secureTokenService.GetTokenAsync();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var result = await client.DeleteAsync($"{baseUri}/Exercises?id={id}");
                if (result.IsSuccessStatusCode)
                {
                    return true;
                }
                return false;
            }
        }

        public async Task<ExerciseCreateResult> AddExercise(Exercise exercise)
        {
            var newExercise = new ExerciseCreateRequestDto
            {
                Name = exercise.Name,
                Description = exercise.Description,
                MuscleGroupId = exercise.MuscleGroupName.Id,
                Reps = exercise.Reps,
                Sets = exercise.Sets,
                Weight = exercise.Weight
            };
            var content = new StringContent(JsonConvert.SerializeObject(newExercise), Encoding.UTF8, "application/json");
            using (var client = new HttpClient())
            {
                var token = await _secureTokenService.GetTokenAsync();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var result = await client.PostAsync($"{baseUri}/Exercises", content);
                if (result.IsSuccessStatusCode)
                {
                    Uri locationUri = result.Headers.Location;
                    int id= Convert.ToInt32(locationUri.Segments.Last());
                    
                    return new ExerciseCreateResult
                    {
                        Succes = true,
                        Id = id,
                    };
                }
                return new ExerciseCreateResult
                {
                    Succes = false,
                };
            }
        }
    }
}
