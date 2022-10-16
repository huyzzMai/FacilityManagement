using DataAccess.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using BusinessObject.Models;
using BusinessObject.RequestModel.RoomRequest;
using System.Xml.Linq;

namespace FacilityManagement.Controllers.RoomController
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService roomService;

        public RoomController(IRoomService roomService)
        {
            this.roomService = roomService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRooms()
        {
            try
            {
                return Ok(await roomService.GetAllRoom());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }
        //[HttpPost]
        //public async Task<IActionResult> Create(string name, [FromBody] RoomRequest rooms)
        //{

        //    try
        //    {
        //        Room r = await roomService.GetRoomByName(name);

        //        if (r == null)
        //        {
        //            await roomService.CreateRoom(name, rooms);
        //            return Ok();
        //        }
        //        else
        //        {
        //            return StatusCode(StatusCodes.Status500InternalServerError,
        //            "Room Existed! ");

        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError,
        //             "Error creating user!");
        //    }
        //}

        [HttpPut("{name:alpha}")]
        public async Task<IActionResult> UpdateRoom(string name, [FromBody] RoomRequest rooms)
        {
            try
            {
                var u = await roomService.GetRoomByName(name);

                if (u == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        "Error retrieving data from the database.");
                }
                else
                {
                    return Ok(await roomService.UpdateRoom(name, rooms));
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error deleting user!");
            }
        }

        [HttpDelete("{name:alpha}")]
        public async Task<IActionResult> DeleteRoom(string name)
        {
            try
            {
                var r = await roomService.GetRoomByName(name);

                if (r == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database.");
                }
                else
                {
                    await roomService.DeleteRoom(name);
                    return Ok();
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                     "Error deleting user!");
            }
        }
        [HttpPost]
        public async Task<IActionResult> TestCreateRoom([FromBody] RoomRequest room)
        {
            try
            {
               
                    await roomService.CreateRoom("1", room);
                    return Ok();
                
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,ex.Message);
            }
        }

        }
    }

