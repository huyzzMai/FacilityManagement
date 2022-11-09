using BusinessObject.RequestModel.UserRequest;
using BusinessObject.ResponseModel.UserResponse;
using DataAccess.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FacilityManagement.Controllers.UserController
{
    [Route("api/personal-user")]
    [ApiController]
    [Authorize(Roles = "Admin,User,Fixer")]
    public class PersonalUserController : ControllerBase
    {
        private readonly IUserService userService;
        public PersonalUserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet("information")]
        public async Task<IActionResult> GetUserInformation()
        {
            try
            {
                // Get id of current log in user 
                int userId = userService.GetCurrentLoginUserId(Request.Headers["Authorization"]);

                var u = await userService.GetUserById(userId);
                var user = new UserInformationRespsonse
                {
                    Email = u.Email,
                    FullName = u.FullName,
                    Image = u.Image,
                };

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

        [HttpPut("information")]
        public async Task<IActionResult> UpdateUser([FromBody] UserRequest model)
        {
            try
            {
                // Get id of current log in user 
                int userId = userService.GetCurrentLoginUserId(Request.Headers["Authorization"]);

                var u = await userService.GetUserById(userId);

                if (u == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database.");
                }
                else
                {
                    await userService.UpdateUser(userId, model);
                    return Ok("Update successfully!");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

        [HttpPut("password")]
        public async Task<IActionResult> UpdatePassword(string oldPass, string newPass)
        {
            try
            {
                // Get id of current log in user 
                int userId = userService.GetCurrentLoginUserId(Request.Headers["Authorization"]);
                await userService.UpdatePassword(userId, oldPass, newPass);
                return Ok("Password updated successfully");
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }
    }
}
