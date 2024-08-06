using MdePe.Domain.Models;
using MdePe.Infrastructure.Dto;
using MdePe.Infrastructure.Dto.Exercises;
using MdePe.Infrastructure.Dto.Workouts;
using MdePe.Infrastructure.Results;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MdePe.Domain.Services.Api
{
    public class ApiWorkoutService : IWorkoutService
    {
        private string baseUri = ApiConstants.BaseUrl;
        private readonly IMuscleGroupService _muscleGroupService;
        private readonly ISecureTokenService _secureTokenService;

        public ApiWorkoutService(IMuscleGroupService muscleGroupService, ISecureTokenService secureTokenService)
        {
            _muscleGroupService = muscleGroupService;
            _secureTokenService = secureTokenService;
        }

        public async Task<Workout> GetWorkoutById(int id)
        {
            using (var client = new HttpClient())
            {
                var token = await _secureTokenService.GetTokenAsync();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var result = await client.GetFromJsonAsync<WorkoutDto>($"{baseUri}/Workouts/{id}");
                if (result == null)
                {
                    return null;
                }
                var workout = new Workout
                {
                    Id = result.Id,
                    Description = result.Description,
                    Duration = result.Duration,
                    Name = result.Name,
                    MuscleGroup = result.MuscleGroup,
                    UserId = result.User.Id,
                    Exercises = result.Exercises.Select(ex => new Exercise
                    {
                        Id = ex.Id,
                        Name = ex.Name,
                        Description = ex.Description,
                        Reps = ex.Reps,
                        Sets = ex.Sets,
                        Weight = ex.Weight,
                    }).ToList(),
                };
                return workout;
            }
        }
        public async Task<bool> Update(Workout workout)
        {
            var updateData = new WorkoutUpdateRequestDto
            {
                Id = workout.Id,
                Name = workout.Name,
                Description = workout.Description,
                MuscleGroupId = workout.MuscleGroup.Id,
                Duration = workout.Duration,
                ExerciseIds = workout.Exercises.Select(e => e.Id).ToList(),
                UserId = workout.UserId,
            };
            var content = new StringContent(JsonConvert.SerializeObject(updateData), Encoding.UTF8, "application/json");
            using (var client = new HttpClient())
            {
                var token = await _secureTokenService.GetTokenAsync();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var result = await client.PutAsync($"{baseUri}/Workouts", content);
                if (result.IsSuccessStatusCode)
                {
                    return true;
                }
                return false;
            }
        }
        public async Task<List<Workout>> GetWorkouts()
        {
            using (var client = new HttpClient())
            {
                var token = await _secureTokenService.GetTokenAsync();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var result = await client.GetFromJsonAsync<WorkoutsDto>($"{baseUri}/Workouts");
                var workouts = new List<Workout>();
                foreach (var workout in result.Workouts)
                {
                    workouts.Add(new Workout
                    {
                        Id = workout.Id,
                        Name = workout.Name,
                        Description = workout.Description,
                        Duration = workout.Duration,
                    });
                }
                return workouts;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using (var client = new HttpClient())
            {
                var token = await _secureTokenService.GetTokenAsync();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var result = await client.DeleteAsync($"{baseUri}/Workouts?id={id}");
                if (result.IsSuccessStatusCode)
                {
                    return true;
                }
                return false;
            }
        }

        public async Task<WorkoutCreateResultModel> CreateAsync(Workout workout)
        {
            var newWorkout = new WorkoutCreateRequestDto
            {
                Name = workout.Name,
                Description = workout.Description,
                Duration = workout.Duration,
                ExerciseIds = workout.Exercises.Select(e => e.Id).ToList(),
                MuscleGroupId = workout.MuscleGroup.Id,
                UserId = workout.UserId
            };
            var content = new StringContent(JsonConvert.SerializeObject(newWorkout), Encoding.UTF8, "application/json");
            using (var client = new HttpClient())
            {
                var token = await _secureTokenService.GetTokenAsync();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var result = await client.PostAsync($"{baseUri}/Workouts", content);
                if (result.IsSuccessStatusCode)
                {
                    Uri locationUri = result.Headers.Location;
                    int id = Convert.ToInt32(locationUri.Segments.Last());

                    return new WorkoutCreateResultModel
                    {
                        Succes = true,
                        Id = id,
                    };
                }
                return new WorkoutCreateResultModel
                {
                    Succes = false,
                };
            }
        }

        public async Task<List<Workout>> GetWorkoutsByUserIdAsync(string id)
        {
            using (var client = new HttpClient())
            {
                var token = await _secureTokenService.GetTokenAsync();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var result = await client.GetFromJsonAsync<WorkoutsDto>($"{baseUri}/Workouts/Search/ByUser/{id}");
                var workouts = new List<Workout>();
                foreach (var workout in result.Workouts)
                {
                    workouts.Add(new Workout
                    {
                        Id = workout.Id,
                        Name = workout.Name,
                        Description = workout.Description,
                        Duration = workout.Duration,
                    });
                }
                return workouts;
            }
        }
    }
}
