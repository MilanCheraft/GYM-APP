using MdePe.Domain.Models;
using MdePe.Infrastructure.Results;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MdePe.Domain.Services
{
    public interface IWorkoutService
    {
        Task<List<Workout>> GetWorkouts();
        Task<Workout> GetWorkoutById(int id);
        Task<bool> Update(Workout workout);
        Task<bool> DeleteAsync(int id);
        Task<WorkoutCreateResultModel> CreateAsync(Workout workout);
        Task<List<Workout>> GetWorkoutsByUserIdAsync(string id);
    }
}
