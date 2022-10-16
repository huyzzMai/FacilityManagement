using BusinessObject.Commons;
using BusinessObject.Models;
using BusinessObject.RequestModel.RoomRequest;
using BusinessObject.ResponseModel.RoomResponse;
using DataAccess.IRepositories;
using DataAccess.IServices;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository roomRepository;

        public IConfiguration _configuration;

        public RoomService(IRoomRepository roomRepository, IConfiguration configuration)
        {
            this.roomRepository = roomRepository;
            _configuration = configuration;
        }
        public async Task<Room> GetRoomByName(string name)
        {
            Room r = await roomRepository.GetRoomByName(name);
            return r;
        }
        public async Task<Room> GetRoomById(int id)
        {
            Room r = await roomRepository.GetRoomAndDeleteIsFalse(id);
            return r;
        }
        public async Task<IEnumerable<RoomResponse>> GetAllRoom()
        {
            var rooms = await roomRepository.GetAllRoom();

            IEnumerable<RoomResponse> result = rooms.Select(
                room =>
                {
                    // Cast status from int to string for response
                    string status;
                    if (room.Status == CommonEnums.ROOMSTATUS.ACTIVE)
                    {
                        status = "Active";
                    }
                    else
                    {
                        status = "InActive";
                    }

                    return new RoomResponse()
                    {
                        Name = room.Name,
                        Level = room.Level,
                        Status = status
                    };
                }
                )
                .ToList();
            return result;
        }
        public async Task<RoomResponse> UpdateRoom(string name, RoomRequest rooms)
        {
            var r = await roomRepository.GetRoomByName(name);

            if (r == null)
            {
                throw new Exception("This room cannot be updated!");
            }

            Room rms = new Room();

            rms.Name = rooms.Name;
            rms.Level = rooms.Level;
            rms.Status = rooms.Status;

            await roomRepository.SaveRoom(rms);

            var uproom = new RoomResponse()
            {
                Name = rms.Name,
                Level = rms.Level,
            };

            return uproom;
        }

        public async Task<RoomResponse> CreateRoom(string name,RoomRequest rooms)
        {
            var r = await roomRepository.GetRoomByName(name);
            
            if (r == null)
            {
                Room rms = new Room();
                rms.Name = rooms.Name;
                rms.Level = rooms.Level;
                rms.Status = rooms.Status;
                await roomRepository.SaveRoom(rms);

                var uproom = new RoomResponse()
                {
                    Name = rms.Name,
                    Level = rms.Level,
                };

                return uproom;
            }
            else
            {
                throw new Exception("This room existed!");
            }

        }
        public async Task DeleteRoom(string name)
        {
            Room r = await roomRepository.GetRoomByName(name);

            if (r == null)
            {
                throw new Exception("This room is unavailable to delete.");
            }

            r.IsDeleted = true;

            await roomRepository.DeleteRoom(r);
        }
    }
}
