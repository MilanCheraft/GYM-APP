using MdePe.Domain.Models;
using MdePe.Infrastructure.Dto.Exercises;
using MdePe.Infrastructure.Dto.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace MdePe.Infrastructure.Dto.Workouts
{
    public class WorkoutDto : BaseDto
    {
        public string Description { get; set; }
        public int Duration { get; set; }
        public UserBaseDto User { get; set; }
        public IEnumerable<ExerciseDto> Exercises { get; set; }
        public MuscleGroup MuscleGroup { get; set; }
    }
}
