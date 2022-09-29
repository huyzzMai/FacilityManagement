using BusinessObject.RequestModel.AuthenticationRequest;
using BusinessObject.ResponseModel.AuthenticationResponse;
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
    }
}
