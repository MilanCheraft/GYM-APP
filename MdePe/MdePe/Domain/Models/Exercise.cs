using System;
using System.Collections.Generic;
using System.Text;

namespace MdePe.Domain.Models
{
    public class Exercise
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Reps { get; set; }
        public int Sets { get; set; }
        public double Weight { get; set; }
        public MuscleGroup MuscleGroupName { get; set; }
        public string ImageUrl { get; set; }

    }
}
