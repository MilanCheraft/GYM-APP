using System;
using System.Collections.Generic;
using System.Text;

namespace MdePe.Infrastructure.Dto.Exercises
{
    public class ExerciseCreateRequestDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int MuscleGroupId { get; set; }
        public int Reps { get; set; }
        public double Weight { get; set; }
        public int Sets { get; set; }
    }
}
