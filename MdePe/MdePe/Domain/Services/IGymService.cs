using MdePe.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MdePe.Domain.Services
{
    public interface IGymService
    {
        Task<IEnumerable<Gym>> GetGymAsync();
    }
}
