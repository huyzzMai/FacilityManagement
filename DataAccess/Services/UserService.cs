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
using BusinessObject.RequestModel.UserRequest;

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

        #region CreateToken
        public LoginResponse CreateToken(Claim[] claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims, expires: DateTime.UtcNow.AddDays(1), signingCredentials: signIn);

            var result = new LoginResponse()
            {
                Token = token
            };
            return result;
        }
        #endregion

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
                    var result = CreateToken(claims);
                    return result;
                } 
                else if (user.Role == CommonEnums.ROLE.FIXER)
                {
                var claims = new[]
                    {
                    new Claim(ClaimTypes.Role, "Fixer"),
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("Id", userId)
                    };
                    var result = CreateToken(claims);
                    return result;
                }
                else
                {
                    var claims = new[]
                    {
                    new Claim(ClaimTypes.Role, "User"),
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("Id", userId)
                    }; 
                    var result = CreateToken(claims);
                    return result;
                }
        }

        public async Task<LoginResponse> LoginUserForGoogle(User user)
        {
            String userId = user.Id.ToString();

            var claims = new[]
                    {
                    new Claim(ClaimTypes.Role, "User"),
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("Id", userId)
                    };
            var result = CreateToken(claims);
            return result;
        }

        public async Task<IEnumerable<UserResponse>> GetAllUsers()
        {
            var users = await userRepository.GetAllUsers();
            IEnumerable<UserResponse> result = users.Select(
                user =>
                {
                    // Cast role from int to string for response
                    string role, status;
                    if (user.Role == CommonEnums.ROLE.ADMIN)
                    {
                        role = "Admin";
                    }
                    else if (user.Role == CommonEnums.ROLE.FIXER)
                    {
                        role = "Fixer";
                    }
                    else
                    {
                        role = "User";
                    }

                    if (user.Status == CommonEnums.USERSTATUS.ACTIVE)
                    {
                        status = "Active";
                    }
                    else
                    {
                        status = "Inactive";
                    }

                    return new UserResponse()
                    {
                        FullName = user.FullName,
                        Email = user.Email,
                        Image = user.Image,
                        Role = role,
                        Status = status
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

        public async Task<User> GetUserByEmail(string email)
        {
            User u = await userRepository.GetUserByEmailAndDeleteIsFalse(email);
            return u;
        }

        public async Task<User> CreateUserByGoogleLogin(GoogleUserCreateRequest request)
        {
            User u = new User()
            {
                FullName = request.FullName,
                Email = request.Email,
                Image = request.Img,
                Role = CommonEnums.ROLE.USER,
                IsDeleted = false,
                CreatedAt = DateTime.Now
            };

            await userRepository.SaveCreateUser(u);

            return u;
        }

        public async Task<EmployeeCreateResponse> CreateEmployee(EmployeeCreateRequest request)
        {
            var u = await userRepository.GetUserByEmailAndDeleteIsFalse(request.Email);

            if (u != null)
            {
                throw new Exception("This user cannot be updated!");
            }
            if (request.Role != CommonEnums.ROLE.FIXER || request.Role != CommonEnums.ROLE.ADMIN)
            {
                throw new Exception("Invalid role!");
            }

            string RandomString(int size, bool lowerCase = false)
            {
                var builder = new StringBuilder(size);

                Random _random = new Random();

                char offset = lowerCase ? 'a' : 'A';
                const int lettersOffset = 26; // A...Z or a..z: length=26  

                for (var i = 0; i < size; i++)
                {
                    var @char = (char)_random.Next(offset, offset + lettersOffset);
                    builder.Append(@char);
                }
                return lowerCase ? builder.ToString().ToLower() : builder.ToString();
            }
            var sb = new StringBuilder();
            sb.Append(RandomString(8, true));
            string pw = sb.ToString();

            User user = new User()
            {
                Email = request.Email,
                Password = pw,
                Role = request.Role,
            };

            await userRepository.SaveCreateUser(user);  

            var rs = new EmployeeCreateResponse
            {
                Email = user.Email,
                Password = user.Password,
            };
            return rs;
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
            u.UpdatedAt = DateTime.Now;

            await userRepository.SaveUser(u);

            var upuser = new UserResponse()
            {
                FullName = u.FullName,
                Email = u.Email,
                Image = u.Image

            };

            return upuser;
        }

        public async Task UpdateUserStatus(int id, int request)
        {
            User u = await userRepository.GetUserAndDeleteIsFalse(id);
            if(u == null)
            {
                throw new Exception("This user is unavailable to update.");
            }

            if (request == CommonEnums.USERSTATUS.BAN)
            {
                if (u.Status != CommonEnums.USERSTATUS.BAN) 
                {
                    u.Status = CommonEnums.USERSTATUS.BAN;
                    u.UpdatedAt = DateTime.Now;
                    u.UpdatedBy = "Admin";
                    await userRepository.SaveUser(u);
                }
                else
                {
                    throw new Exception("This user already ban!");
                }
            }
            else if (request == CommonEnums.USERSTATUS.REMOVEBAN)
            {
                if (u.Status == CommonEnums.USERSTATUS.BAN)
                {
                    u.Status = CommonEnums.USERSTATUS.ACTIVE;
                    u.UpdatedAt = DateTime.Now;
                    u.UpdatedBy = "Admin";
                    await userRepository.SaveUser(u);
                }
                else
                {
                    throw new Exception("This user already active!");
                }
            }
            else
            {
                throw new Exception("Action can not be executed!");
            }
        }

        public async Task DeleteUser(int id)
        {
            User u = await userRepository.GetUserAndDeleteIsFalse(id);

            if(u == null)
            {
                throw new Exception("This user is unavailable to delete.");
            }
            
            u.IsDeleted = true;
            u.Status = CommonEnums.USERSTATUS.INACTIVE;
            u.UpdatedAt = DateTime.Now;

            await userRepository.SaveUser(u);
        }

    }
}
