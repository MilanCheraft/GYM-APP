using MdePe.Domain.Models;
using MdePe.Infrastructure.Dto.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MdePe.Domain.Services
{
    public interface IUserService
    {
        Task<User> GetUserByEmail(string email);
        Task<bool> Login(string username, string password);
        bool Logout();
        Task<bool> Register(User newUser);
        Task<bool> Update(User userUpdate);
        Task<IEnumerable<BaseUserdto>> GetUsersAsync();
    }
}
