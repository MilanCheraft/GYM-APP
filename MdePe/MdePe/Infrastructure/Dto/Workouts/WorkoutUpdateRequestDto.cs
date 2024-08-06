using System;
using System.Collections.Generic;
using System.Text;

namespace MdePe.Infrastructure.Dto.Workouts
{
    public class WorkoutUpdateRequestDto : WorkoutCreateRequestDto
    {
        public int Id { get; set; }

    }
}
