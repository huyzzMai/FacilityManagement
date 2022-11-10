using DataAccess.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using BusinessObject.Models;
using BusinessObject.RequestModel.RoomRequest;
using Microsoft.AspNetCore.Authorization;
using BusinessObject.Commons;
using BusinessObject.ResponseModel.RoomResponse;


namespace FacilityManagement.Controllers.RoomController
{
    [Route("api/room")]
    [ApiController]
    [Authorize(Roles = "Admin, User, Fixer")]
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
        //[Authorize(Roles = "Admin")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetRoomById(int id)
        {
            try
            {
                var d = await roomService.GetRoomById(id);
                if (d == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                    "Can not found this id in database.");
                }

                string status = null;
                if (d.Status == CommonEnums.ROOMSTATUS.ACTIVE)
                {
                    status = "ACTIVE";
                }
                else if (d.Status == CommonEnums.ROOMSTATUS.INACTIVE)
                {
                    status = "INACTIVE";
                }

                var room = new RoomResponse()
                {
                    Name = d.Name,
                    Level = d.Level,
                    Status = status
                };

                return Ok(room);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RoomRequest rooms)
        {

            try
            {
                var r = await roomService.GetRoomByName(rooms.Name);

                if (r == null)
                {
                    
                    return StatusCode(StatusCodes.Status201Created,
                    await roomService.CreateRoom(rooms));
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

        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateRoom(int id,[FromBody] RoomRequest rooms)
        {
            try
            {
                var u = await roomService.GetRoomById(id);

                if (u == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        "Error retrieving data from the database.");
                }
                else
                {
                    await roomService.UpdateRoom(id, rooms);
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
        [HttpPut("status-inactive/{id:int}")]
        public async Task<IActionResult> UpdateRoomStatus(int id)
        {
            try
            {
                var u = await roomService.GetRoomById(id);

                if (u == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        "Error retrieving data from the database.");
                }
                else
                {
                    await roomService.UpdateRoomStatusInActive(id);
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
        [HttpPut("status-active/{id:int}")]
        public async Task<IActionResult> RemoveRoomStatusInActive(int id)
        {
            try
            {
                var u = await roomService.GetRoomById(id);

                if (u == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        "Error retrieving data from the database.");
                }
                else
                {
                    await roomService.RemoveRoomStatusInActive(id);
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
        public async Task<IActionResult> DeleteRoom(int id)
        {
            try
            {
                var r = await roomService.GetRoomById(id);

                if (r == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database.");
                }
                else
                {
                    await roomService.DeleteRoom(id);
                    return Ok("This room have been deleted successfully");
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

