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
using System.Security.Cryptography;
using System.IO;

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

        #region Encrypt Password
        public string EncryptPassword(string plainText)
        {
            var key = "b14ca5898a4e4133bbce2ea2315a1916";
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }
        #endregion

        #region Decrypt Password
        public string DecryptPassword(string plainText)
        {
            var key = "b14ca5898a4e4133bbce2ea2315a1916";
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(plainText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }
        #endregion

        public int GetCurrentLoginUserId(string authHeader)
        {
            var handler = new JwtSecurityTokenHandler();
            authHeader = authHeader.Replace("Bearer ", "");
            var jsonToken = handler.ReadToken(authHeader);
            var tokenS = handler.ReadToken(authHeader) as JwtSecurityToken;
            var id = tokenS.Claims.First(claim => claim.Type == "nameid").Value;
            int userId = int.Parse(id);
            return userId;
        }

        public async Task<LoginResponse> LoginUser(LoginRequest request)
        {
            User u = await userRepository.GetUserByEmailAndDeleteIsFalse(request.Email);
            var decryptPass = DecryptPassword(u.Password); 

            if (request.Password != decryptPass)
            {
                throw new Exception("Wrong password!");
            }

                String userId = u.Id.ToString();

                if (u.Role == CommonEnums.ROLE.ADMIN)
                {
                    var claims = new[]
                    {
                    new Claim(ClaimTypes.Role, "Admin"),
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim(JwtRegisteredClaimNames.NameId, userId)
                    };
                    var result = CreateToken(claims);
                    return result;
                } 
                else if (u.Role == CommonEnums.ROLE.FIXER)
                {
                var claims = new[]
                    {
                    new Claim(ClaimTypes.Role, "Fixer"),
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim(JwtRegisteredClaimNames.NameId, userId)
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
                    new Claim(JwtRegisteredClaimNames.NameId, userId)
                    }; 
                    var result = CreateToken(claims);
                    return result;
                }
        }

        public async Task RegisterUser(RegisterRequest request)
        {
            var u = await userRepository.GetUserByEmailAndDeleteIsFalse(request.Email);

            if (u != null)
            {
                throw new Exception("Email has been used!");
            }

            var encryPass = EncryptPassword(request.Password);

            User user = new User()
            {
                FullName = request.FullName,
                Email = request.Email,
                Password = encryPass,
                DepartmentId = CommonEnums.DEPARTMENTID.USERDEPARTMENT,
                Role = CommonEnums.ROLE.USER,
                Status = CommonEnums.USERSTATUS.ACTIVE,
                IsDeleted = false,
                CreatedAt = DateTime.Now,
                CreatedBy = "system"
            };

            await userRepository.SaveCreateUser(user);
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
                        status = "ACTIVE";
                    }
                    else
                    {
                        status = "BAN";
                    }

                    return new UserResponse()
                    {
                        Id = user.Id,
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
                throw new Exception("Email has been used!");
            }
            if (request.Role != CommonEnums.ROLE.FIXER && request.Role != CommonEnums.ROLE.ADMIN)
            {
                throw new Exception("Invalid role!");
            }

            // Generate password
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

            string encryptPassword = EncryptPassword(pw);

            int department;
            if(request.Role == CommonEnums.ROLE.ADMIN)
            {
                department = CommonEnums.DEPARTMENTID.ADMINDEPARTMENT;
            }
            else
            {
                department = CommonEnums.DEPARTMENTID.MAINTENANCEDEPARTMENT;
            }

            User user = new User()
            {
                Email = request.Email,
                Password = encryptPassword,
                Role = request.Role,
                DepartmentId = department,
                Status = CommonEnums.USERSTATUS.ACTIVE,
                IsDeleted = false,
                CreatedAt = DateTime.Now,
                CreatedBy = "admin"
            };

            await userRepository.SaveCreateUser(user);  

            var rs = new EmployeeCreateResponse
            {
                Email = user.Email,
                Password = pw,
            };
            return rs;
        }

        public async Task UpdateUser(int id, UserRequest model)
        {
            var u = await userRepository.GetUserAndDeleteIsFalse(id);

            if(u == null)
            {
                throw new Exception("This user cannot be updated!");
            }

            var check = await userRepository.GetUserByEmailAndDeleteIsFalse(model.Email);
            if(check != null)
            {
                throw new Exception("This email existed!");
            }

            if (model.FullName == null)
            {
                u.FullName = u.FullName;
            }
            else
            {
                u.FullName = model.FullName;
            }

            if (model.Email == null)
            {
                u.Email = u.Email;
            }
            else {
                u.Email = model.Email;
            }

            if (model.Email == null)
            {
                u.Image = u.Image;
            }
            else
            {
                u.Image = model.Image;
            }

            u.UpdatedAt = DateTime.Now;
            u.UpdatedBy = "system";

            await userRepository.SaveUser(u);

            //var upuser = new UserResponse()
            //{
            //    FullName = u.FullName,
            //    Email = u.Email,
            //    Image = u.Image

            //};

            //return upuser;
        }

        public async Task BanUser(int id)
        {
            User u = await userRepository.GetUserAndDeleteIsFalse(id);
            if(u == null)
            {
                throw new Exception("This user is unavailable to update.");
            }

                if (u.Status != CommonEnums.USERSTATUS.BAN) 
                {
                    u.Status = CommonEnums.USERSTATUS.BAN;
                    u.UpdatedAt = DateTime.Now;
                    u.UpdatedBy = "admin";
                    await userRepository.SaveUser(u);
                }
                else
                {
                    throw new Exception("This user already ban!");
                }
        }

        public async Task RemoveBanUser(int id)
        {
            User u = await userRepository.GetUserAndDeleteIsFalse(id);
            if (u == null)
            {
                throw new Exception("This user is unavailable to update.");
            }

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
            u.UpdatedBy = "admin";

            await userRepository.SaveUser(u);
        }

    }
}
