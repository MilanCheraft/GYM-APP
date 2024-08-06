using System;
using System.Collections.Generic;
using System.Text;

namespace MdePe.Domain.Models
{
    public class Gym
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public double longitude { get; set; }
        public double latitude { get; set; }
    }
}
