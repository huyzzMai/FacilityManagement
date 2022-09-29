using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.ResponseModel.AuthenticationResponse
{
    public class LoginResponse
    {
        public SecurityToken Token { get; set; }

    }
}
