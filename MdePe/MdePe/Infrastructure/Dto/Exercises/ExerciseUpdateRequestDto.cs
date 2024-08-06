using System;
using System.Collections.Generic;
using System.Text;

namespace MdePe.Infrastructure.Dto.Exercises
{
    public class ExerciseUpdateRequestDto : ExerciseCreateRequestDto
    {
        public int Id { get; set; }
    }
}
