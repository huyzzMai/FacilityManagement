using BusinessObject.Models;
using BusinessObject.RequestModel.AuthenticationRequest;
using BusinessObject.RequestModel.UserRequest;
using DataAccess.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
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

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
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
            }catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

        [HttpPost("user-register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                await userService.RegisterUser(request);
                return StatusCode(StatusCodes.Status201Created,
                       "Register successfully!");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

        //[Route("api/authen/google-login")]
        //[HttpPost]
        //[AllowAnonymous]
        //public async Task<IActionResult> GoogleLogin()
        //{
        //    try
        //    {
        //        var properties = new AuthenticationProperties { RedirectUri = Url.Page("localhost:5000/api/authen/google-login", pageHandler: "GoogleResponse") };
        //        return Challenge(GoogleDefaults.AuthenticationScheme);

        //        var result = await HttpContext.AuthenticateAsync();
        //        var claims = result.Principal.Identities.FirstOrDefault().Claims;
        //        var identity = new ClaimsIdentity(claims);
        //        var principle = new ClaimsPrincipal(identity);
        //        string email = principle.FindFirstValue(ClaimTypes.Email);
        //        string displayName = principle.FindFirstValue(ClaimTypes.Name);
        //        string avatar = principle.FindFirstValue("urn:google:picture");

        //        User user = await userService.GetUserByEmail(email);

        //        if (user == null)
        //        {
        //            GoogleUserCreateRequest request = new GoogleUserCreateRequest()
        //            {
        //                FullName = displayName,
        //                Email = email,
        //                Img = avatar,
        //            };

        //            var u = await userService.CreateUserByGoogleLogin(request);

        //            var response = await userService.LoginUserForGoogle(u);
        //            var token = response.Token;
        //            if (token != null)
        //            {
        //                return Ok(new JwtSecurityTokenHandler().WriteToken(token));

        //            }
        //            else
        //            {
        //                return BadRequest("Invalid Credentials");
        //            }
        //        }
        //        else
        //        {
        //            var response = await userService.LoginUserForGoogle(user);
        //            var token = response.Token;
        //            if (token != null)
        //            {
        //                return Ok(new JwtSecurityTokenHandler().WriteToken(token));

        //            }
        //            else
        //            {
        //                return BadRequest("Invalid Credentials");
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError,
        //            ex.Message);
        //    }
        //}

    }
}
