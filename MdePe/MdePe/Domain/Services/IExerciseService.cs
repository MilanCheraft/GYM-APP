using MdePe.Domain.Models;
using MdePe.Infrastructure.Results;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MdePe.Domain.Services
{
    public interface IExerciseService
    {
        Task<bool> DeleteExerciseAsync(int id);
        Task<List<Exercise>> GetExercises();
        Task<Exercise> GetExercisesById(int id);
        Task<bool> UpdateExerciseAsync(Exercise exercise);
        Task<ExerciseCreateResult> AddExercise(Exercise exercise);
    }
}
