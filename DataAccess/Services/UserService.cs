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

                if (user.Role == 1)
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
        }

    }
}
