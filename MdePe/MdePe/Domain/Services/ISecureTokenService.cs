using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MdePe.Domain.Services
{
    public interface ISecureTokenService
    {
        List<Claim> GetClaims(string token);
        Task<string> GetTokenAsync();
        Task<string> GetUserRole();
        bool RemoveToken();
        Task<bool> SaveTokenAsync(string token);
    }
}
