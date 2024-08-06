using MdePe.Domain.Models;
using MdePe.Infrastructure.Results;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MdePe.Domain.Services
{
    public interface IMuscleGroupService
    {
        Task<List<MuscleGroup>> GetMuscleGroups();
        Task<MuscleGroup> GetMuscleGroupById(int id);
    }
}
