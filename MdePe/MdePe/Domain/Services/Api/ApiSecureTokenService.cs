using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace MdePe.Domain.Services.Api
{
    public class ApiSecureTokenService : ISecureTokenService
    {
        public async Task<bool> SaveTokenAsync(string token)
        {
            try
            {
                await SecureStorage.SetAsync("bearerToken", token);
                return true;
            }
            catch
            {
                return false;
            }

        }
        public async Task<string> GetTokenAsync()
        {
            return await SecureStorage.GetAsync("bearerToken");
        }
        public List<Claim> GetClaims(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            return jwtToken.Claims.ToList();
        }
        public async Task<string> GetUserRole()
        {
            return await SecureStorage.GetAsync("Role");
        }
        public bool RemoveToken()
        {
            try
            {
                SecureStorage.RemoveAll();
                return true;
            }
            catch
            {
                return false;
            }

        }
    }
}
