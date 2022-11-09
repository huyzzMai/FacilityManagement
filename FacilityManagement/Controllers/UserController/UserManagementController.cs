using BusinessObject.RequestModel.UserRequest;
using DataAccess.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FacilityManagement.Controllers.UserController
{
    [Route("api/user")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UserManagementController : ControllerBase
    {
        private readonly IUserService userService;
        public UserManagementController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                return Ok(await userService.GetAllUsers());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee([FromBody] EmployeeCreateRequest model)
        {
            try
            {
                return StatusCode(StatusCodes.Status201Created,
                    await userService.CreateEmployee(model));
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

        //[HttpPut("{id:int}")]
        //public async Task<IActionResult> UpdateUser(int id, [FromBody] UserRequest model)
        //{
        //    try
        //    {
        //        var u = await userService.GetUserById(id);

        //        if (u == null)
        //        {
        //            return StatusCode(StatusCodes.Status500InternalServerError,
        //            "Error retrieving data from the database.");
        //        }
        //        else
        //        {
        //            return Ok(await userService.UpdateUser(id, model));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError,
        //            ex.Message);
        //    }
        //}

        [HttpPut("status/ban/{id:int}")]
        public async Task<IActionResult> BanUser(int id)
        {
            try
            {
                await userService.BanUser(id);
                return Ok("Status updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

        [HttpPut("status/remove-ban/{id:int}")]
        public async Task<IActionResult> RemoveBanUser(int id)
        {
            try
            {
                await userService.RemoveBanUser(id);
                return Ok("Remove ban user successfully.");
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteUser(int id)
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
                    await userService.DeleteUser(id);
                    return Ok("User deleted successfully!");
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
