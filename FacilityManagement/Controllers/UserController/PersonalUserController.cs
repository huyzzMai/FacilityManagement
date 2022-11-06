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
    [Route("api/personaluser")]
    [ApiController]
    //[Authorize(Roles = "Admin, User, Fixer")]
    public class PersonalUserController : ControllerBase
    {
        private readonly IUserService userService;
        public PersonalUserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetUserInformation(int id)
        {
            try
            {
                var u = await userService.GetUserById(id);
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

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserRequest model)
        {
            try
            {
                var u = await userService.GetUserById(id);

                if (u == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database.");
                }
                else
                {
                    return Ok(await userService.UpdateUser(id, model));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }
    }
}
