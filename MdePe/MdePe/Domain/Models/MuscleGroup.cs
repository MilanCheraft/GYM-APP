using System;
using System.Collections.Generic;
using System.Text;

namespace MdePe.Domain.Models
{
    public class MuscleGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
