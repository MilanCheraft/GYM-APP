using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MdePe.Infrastructure.Dto.Exercises
{
    public class ExerciseDto
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("muscleGroupId")]
        public int MuscleGroupId { get; set; }

        [JsonProperty("reps")]
        public int Reps { get; set; }

        [JsonProperty("sets")]
        public int Sets { get; set; }

        [JsonProperty("weight")]
        public int Weight { get; set; }
    }
}
