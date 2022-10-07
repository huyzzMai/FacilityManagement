using BusinessObject.Models;
using BusinessObject.RequestModel.AuthenticationRequest;
using BusinessObject.ResponseModel.AuthenticationResponse;
using DataAccess.IRepositories;
using DataAccess.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using BusinessObject.Commons;
using BusinessObject.ResponseModel.UserResponse;
using BusinessObject.RequestModel.UserReqest;

namespace DataAccess.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;

        public IConfiguration _configuration;

        public UserService(IUserRepository userRepository, IConfiguration configuration)
        {
            this.userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<LoginResponse> LoginUser(LoginRequest request)
        {
            User user = await userRepository.GetUserByEmailAndPassword(request.Email, request.Password);
            
                String userId = user.Id.ToString();

                if (user.Role == CommonEnums.ROLE.ADMIN)
                {
                    var claims = new[]
                    {
                    new Claim(ClaimTypes.Role, "Admin"),
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("Id", userId)
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims, expires: DateTime.UtcNow.AddDays(1), signingCredentials: signIn);

                    var result = new LoginResponse()
                    {
                        Token = token
                    };
                    return result;
                }
                else
                {
                    var claims = new[]
                    {
                    new Claim(ClaimTypes.Role, "Student"),
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("Id", userId)
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims, expires: DateTime.UtcNow.AddDays(1), signingCredentials: signIn);

                    var result = new LoginResponse()
                    {
                        Token = token
                    };
                    return result;
                }
        }

        public async Task<IEnumerable<UserResponse>> GetAllUsers()
        {
            var users = await userRepository.GetAllUsers();
            IEnumerable<UserResponse> result = users.Select(
                user =>
                {
                    // Cast role from int to string for response
                    string role;
                    if (user.Role == CommonEnums.ROLE.ADMIN)
                    {
                        role = "Admin";
                    }
                    else
                    {
                        role = "Student";
                    }

                    return new UserResponse()
                    {
                        FullName = user.FullName,
                        Email = user.Email,
                        Image = user.Image,
                        Role = role,
                        Status = user.Status
                    };
                }
                )
                .ToList();
            return result;  
        }

        public async Task<User> GetUserById(int id)
        {
            User u = await userRepository.GetUserAndDeleteIsFalse(id);
            return u;
        }

        public async Task<UserResponse> UpdateUser(int id, UserRequest model)
        {
            var u = await userRepository.GetUserAndDeleteIsFalse(id);

            if(u == null)
            {
                throw new Exception("This user cannot be updated!");
            }

            u.FullName = model.FullName;
            u.Email = model.Email;
            u.Image = model.Image;

            await userRepository.SaveUser(u);

            var upuser = new UserResponse()
            {
                FullName = u.FullName,
                Email = u.Email,
                Image = u.Image

            };

            return upuser;
        }

        public async Task DeleteUser(int id)
        {
            User u = await userRepository.GetUserAndDeleteIsFalse(id);

            if(u == null)
            {
                throw new Exception("This user is unavailable to delete.");
            }
            
            u.IsDeleted = true;

           await userRepository.SaveUser(u);
        }

    }
}
