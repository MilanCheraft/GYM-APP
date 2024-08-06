using System;
using System.Collections.Generic;
using System.Text;

namespace MdePe.Domain.Models
{
    public class Workout
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public IEnumerable<Exercise> Exercises { get; set; }
        public MuscleGroup MuscleGroup { get; set;}
        public string UserId { get; set; }

    }
}
