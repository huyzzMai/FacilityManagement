using BusinessObject.Models;
using BusinessObject.RequestModel.AuthenticationRequest;
using BusinessObject.RequestModel.UserRequest;
using BusinessObject.ResponseModel.AuthenticationResponse;
using BusinessObject.ResponseModel.UserResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IServices
{
    public interface IUserService
    {
        int GetCurrentLoginUserId(string authHeader);
        Task<LoginResponse> LoginUser(LoginRequest request);
        Task<LoginResponse> LoginUserForGoogle(User user);
        Task<IEnumerable<UserResponse>> GetAllUsers();
        Task<User> GetUserById(int id);
        Task<User> GetUserByEmail(string email);
        Task<User> CreateUserByGoogleLogin(GoogleUserCreateRequest request);
        Task <EmployeeCreateResponse> CreateEmployee(EmployeeCreateRequest request);  
        Task<UserResponse> UpdateUser(int id, UserRequest model);
        Task UpdateUserStatus(int id, int request);
        Task DeleteUser(int id);

    }
}
