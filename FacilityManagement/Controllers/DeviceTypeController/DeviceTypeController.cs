using DataAccess.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using BusinessObject.RequestModel.DeviceTypeRequest;
using DataAccess.Services;

namespace FacilityManagement.Controllers.DeviceType
{
    [Route("api/device-type")]
    [ApiController]
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
                    return Ok(await deviceTypeService.UpdateDeviceType(id, devices));

                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

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
                    return Ok();
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
