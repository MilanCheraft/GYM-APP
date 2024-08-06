using FreshMvvm;
using MdePe.Domain.Models;
using MdePe.Infrastructure.Dto;
using MdePe.Infrastructure.Dto.Users;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MdePe.Domain.Services.Api
{
    public class ApiUserService : IUserService
    {
        private string baseUri = ApiConstants.BaseUrl;
        private readonly ISecureTokenService _secureTokenService;

        public ApiUserService(ISecureTokenService secureTokenService)
        {
            _secureTokenService = secureTokenService;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            using(var client = new HttpClient())
            {
                var token = await _secureTokenService.GetTokenAsync();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var result = await client.GetFromJsonAsync<UserDto>($"{baseUri}/User/Search/{email}");
                if(result == null)
                {
                    return null;
                }
                var user = new User
                {
                    Id = result.UserId,
                    Email = result.UserName,
                    DisplayName = result.DisplayName,
                    FirstName = result.FirstName,
                    LastName = result.LastName,
                    DateOfBirth = result.BirthDate,
                    Length = result.Length,
                    Weight = result.Weight
                };
                return user;
            }
        }

        public async Task<bool> Login(string username, string password)
        {
            _secureTokenService.RemoveToken();
            Application.Current.Properties.Clear();
            
            var loginData = new LoginRequestDto
            {
                Email = username,
                Password = password
            };
            var content = new StringContent(JsonConvert.SerializeObject(loginData), Encoding.UTF8, "application/json");
            using (var client = new HttpClient())
            {
                var result = await client.PostAsync($"{baseUri}/User/Login", content);
                    if (result.IsSuccessStatusCode)
                    {
                        var jsonString = await result.Content.ReadAsStringAsync();
                        var loginDto = JsonConvert.DeserializeObject<LoginDto>(jsonString);
                    if(!await _secureTokenService.SaveTokenAsync(loginDto.BearerToken))
                    {
                        return false;
                    }
                    var claims = _secureTokenService.GetClaims(loginDto.BearerToken);
                    var role = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
                    await SecureStorage.SetAsync("Role", role);
                    Application.Current.Properties.Add("Username", username);
                    return true;
                }
            }            
            return false;
        }

        public bool Logout()
        {
            try
            {
                
                Application.Current.Properties.Remove("Username");
                if(!_secureTokenService.RemoveToken())
                {
                    return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> Register(User newUser)
        {
            var registerData = new RegisterRequestDto
            {
                BirthDate = newUser.DateOfBirth,
                DisplayName = newUser.DisplayName,
                Email = newUser.Email,
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                Length = newUser.Length,
                Password = newUser.Password,
                RepeatPassword = newUser.Password,
                Weight = newUser.Weight                
            };
            var content = new StringContent(JsonConvert.SerializeObject(registerData), Encoding.UTF8, "application/json");
            using (var client = new HttpClient())
            {
                var result = await client.PostAsync($"{baseUri}/User/Register", content);
                if (result.IsSuccessStatusCode)
                {

                    return true;
                }
            }
            return false;
        }

        public async Task<bool> Update(User userUpdate)
        {
            var userDto = new UserUpdateDto
            {
                Id = userUpdate.Id,
                DisplayName = userUpdate.DisplayName,
                Dob = userUpdate.DateOfBirth,
                FirstName = userUpdate.FirstName,
                LastName = userUpdate.LastName,
                Weight = userUpdate.Weight,
                Length = userUpdate.Length
            };
            var content = new StringContent(JsonConvert.SerializeObject(userDto), Encoding.UTF8, "application/json");
            using (var client = new HttpClient())
            {
                var token = await _secureTokenService.GetTokenAsync();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var result = await client.PutAsync($"{baseUri}/User", content);
                if (result.IsSuccessStatusCode)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<IEnumerable<BaseUserdto>> GetUsersAsync()
        {
            using (var client = new HttpClient())
            {
                var token = await _secureTokenService.GetTokenAsync();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var result = await client.GetFromJsonAsync<UsersDto>($"{baseUri}/User");
                if (result.Users.Count() <= 0)
                {
                    return null;
                }
                return result.Users;
                
            }
            
        }
    }
}
