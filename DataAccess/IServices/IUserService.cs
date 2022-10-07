using BusinessObject.Models;
using BusinessObject.RequestModel.AuthenticationRequest;
using BusinessObject.RequestModel.UserReqest;
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
        Task<LoginResponse> LoginUser(LoginRequest request);
        Task<IEnumerable<UserResponse>> GetAllUsers();
        Task<User> GetUserById(int id);
        Task<UserResponse> UpdateUser(int id, UserRequest model);
        Task DeleteUser(int id);

    }
}
