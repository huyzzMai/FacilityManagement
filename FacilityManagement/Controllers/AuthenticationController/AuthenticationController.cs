using BusinessObject.RequestModel.AuthenticationRequest;
using DataAccess.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace FacilityManagement.Controllers.AuthenticationController
{
    [Route("api/authen")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserService userService;

        public AuthenticationController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var response = await userService.LoginUser(request); 
            var token = response.Token;
            if (token != null)
            {
                  return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                
            }
            else
            {
                return BadRequest("Invalid Credentials");    
            }
        }

    }
}
