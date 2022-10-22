﻿using DataAccess.IServices;
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
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RoomRequest rooms)
        {

            try
            {
                var r = await roomService.GetRoomByName(rooms.Name);

                if (r == null)
                {
                    await roomService.CreateRoom(rooms);
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
                    return Ok(await roomService.UpdateRoom(id, rooms));

                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

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
