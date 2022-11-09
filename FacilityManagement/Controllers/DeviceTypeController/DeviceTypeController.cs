using DataAccess.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using BusinessObject.RequestModel.DeviceTypeRequest;
using DataAccess.Services;
using BusinessObject.Commons;
using BusinessObject.ResponseModel.DeviceTypeResponse;

namespace FacilityManagement.Controllers.DeviceType
{
    [Route("api/device-type")]
    [ApiController]
    //[Authorize(Roles = "Admin, User")]
    public class DeviceTypeController : ControllerBase
    {
        private readonly IDeviceTypeService deviceTypeService;

        public DeviceTypeController(IDeviceTypeService deviceTypeService)
        {
            this.deviceTypeService = deviceTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDeviceType()
        {
            try
            {
                return Ok(await deviceTypeService.GetAllDeviceType());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }
        //[Authorize(Roles = "Admin")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetDeviceTypeById(int id)
        {
            try
            {
                var d = await deviceTypeService.GetDeviceTypeById(id);
                if (d == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                    "Can not found this id in database.");
                }

                var device = new DeviceTypeResponse()
                {
                    DepartmentId = d.DepartmentId,
                    Name = d.Name,
                };

                return Ok(device);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateDeviceType([FromBody] DeviceTypeRequest devices)
        {

            try
            {
                var r = await deviceTypeService.GetDeviceTypeByName(devices.Name);

                if (r == null)
                {
                    
                    return StatusCode(StatusCodes.Status201Created,
                    await deviceTypeService.CreateDeviceType(devices));
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                    "Room Existed! ");

                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                     "Error creating user!");
            }
        }
        //[Authorize(Roles = "Admin")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateDeviceType(int id, [FromBody] DeviceTypeRequest devices)
        {
            try
            {
                var u = await deviceTypeService.GetDeviceTypeById(id);

                if (u == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        "Error retrieving data from the database.");
                }
                else
                {
                    await deviceTypeService.UpdateDeviceType(id, devices);
                    return Ok("Update Successfully!");

                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }
        //[Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteDeviceType(int id)
        {
            try
            {
                var r = await deviceTypeService.GetDeviceTypeById(id);

                if (r == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database.");
                }
                else
                {
                    await deviceTypeService.DeleteDeviceType(id);
                    return Ok("This Device Type have been deleted successfully");
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                     "Error deleting user!");
            }
        }
    }
}
