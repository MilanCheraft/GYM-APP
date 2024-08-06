using System;
using System.Collections.Generic;
using System.Text;

namespace MdePe.Infrastructure.Dto.Workouts
{
    public class WorkoutCreateRequestDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public List<int> ExerciseIds { get; set; }
        public int MuscleGroupId { get; set; }
        public string UserId { get; set; }
    }
}
