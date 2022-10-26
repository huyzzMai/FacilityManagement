using BusinessObject.RequestModel.DeviceRequest;
using DataAccess.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;

namespace FacilityManagement.Controllers.DeviceController
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly IDeviceService deviceService;

        public DeviceController(IDeviceService deviceService)
        {
            this.deviceService = deviceService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDevice()
        {
            try
            {
                return Ok(await deviceService.GetAllDevice());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateDevice([FromBody] DeviceRequest devices)
        {

            try
            {
                var r = await deviceService.GetDeviceByName(devices.Name);

                if (r == null)
                {
                    await deviceService.CreateDevice(devices);
                    return Ok();
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
        public async Task<IActionResult> UpdateDevice(int id, [FromBody] DeviceRequest devices)
        {
            try
            {
                var u = await deviceService.GetDeviceById(id);

                if (u == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        "Error retrieving data from the database.");
                }
                else
                {
                    return Ok(await deviceService.UpdateDevice(id, devices));

                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteDevice(int id)
        {
            try
            {
                var r = await deviceService.GetDeviceById(id);

                if (r == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database.");
                }
                else
                {
                    await deviceService.DeleteDevice(id);
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
