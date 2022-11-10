using BusinessObject.RequestModel.DeviceRequest;
using DataAccess.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using DataAccess.Services;
using BusinessObject.Commons;
using BusinessObject.ResponseModel.DeviceResponse;
using Microsoft.AspNetCore.Authorization;

namespace FacilityManagement.Controllers.DeviceController
{
    [Route("api/device")]
    [ApiController]
    [Authorize(Roles = "Admin, User")]
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
        //[Authorize(Roles = "Admin")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetDeviceById(int id)
        {
            try
            {
                var d = await deviceService.GetDeviceById(id);
                if (d == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                    "Can not found this id in database.");
                }

                string status = null;
                if (d.Status == CommonEnums.DEVICESTATUS.ACTIVE)
                {
                    status = "ACTIVE";
                }
                else if (d.Status == CommonEnums.DEVICESTATUS.INACTIVE)
                {
                    status = "INACTIVE";
                }

                var device = new DeviceResponse()
                {
                    DeviceTypeID = d.DeviceTypeId,
                    RoomID = d.RoomId,
                    Name = d.Name,
                    Status = d.Status
                };

                return Ok(device);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateDevice([FromBody] DeviceRequest devices)
        {

            try
            {
                var r = await deviceService.GetDeviceByName(devices.Name);

                if (r == null)
                {
                    
                    return StatusCode(StatusCodes.Status201Created,
                    await deviceService.CreateDevice(devices)); 
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                    "Room Existed! ");

                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }
        [Authorize(Roles = "Admin")]
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
                    await deviceService.UpdateDevice(id, devices);
                    return Ok("Update Successfully");

                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }
        [Authorize(Roles = "Admin")]
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
                    return Ok("This Device have been deleted successfully");
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
