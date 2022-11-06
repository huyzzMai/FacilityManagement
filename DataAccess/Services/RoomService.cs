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
        public async Task<RoomResponse> UpdateRoom(int id, RoomRequest rooms)
        {
            var r = await roomRepository.GetRoomAndDeleteIsFalse(id);
            var u = await roomRepository.GetRoomByName(rooms.Name);

            if (r == null)
            {
                throw new Exception("This room cannot be updated because there is no room with that id!");
            }
            if (u == null)
            {
                r.Name = rooms.Name;
                r.Level = rooms.Level;
                r.Status = rooms.Status;
                r.UpdatedAt = DateTime.Now;

                await roomRepository.UpdateRoom(r);

                var uproom = new RoomResponse()
                {
                    Name = r.Name,
                    Level = r.Level,
                };

                return uproom;
            }
            else
            {
                throw new Exception("Room already existed!");
            }
        }

        public async Task<RoomResponse> CreateRoom(RoomRequest rooms)
        {
            var r = await roomRepository.GetRoomByName(rooms.Name);
            
            if (r == null)
            {
                Room rms = new Room();
                rms.Name = rooms.Name;
                rms.Level = rooms.Level;
                rms.Status = rooms.Status;
                rms.IsDeleted = false;
                rms.CreatedAt = DateTime.Now;
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
        public async Task DeleteRoom(int id)
        {
            Room r = await roomRepository.GetRoomAndDeleteIsFalse(id);

            if (r == null)
            {
                throw new Exception("This room is unavailable to delete.");
            }

            r.IsDeleted = true;

            await roomRepository.UpdateRoom(r);
        }

        public async Task AddFixingStatus(int id)
        {
            Room r = await roomRepository.GetRoomAndDeleteIsFalse(id);
            if (r == null)
            {
                throw new Exception("This room is fixing.");
            }

            if (r.Status != CommonEnums.ROOMSTATUS.FIXING)
            {
                r.Status = CommonEnums.ROOMSTATUS.FIXING;
                r.UpdatedAt = DateTime.Now;
                r.UpdatedBy = "Admin";
                await roomRepository.SaveRoom(r);
            }
            else
            {
                throw new Exception("This room is fixing !");
            }
        }

        public async Task AddFixedStatus(int id)
        {
            Room r = await roomRepository.GetRoomAndDeleteIsFalse(id);
            if (r == null)
            {
                throw new Exception("This room is already fixed.");
            }

            if (r.Status != CommonEnums.ROOMSTATUS.FIXED)
            {
                r.Status = CommonEnums.ROOMSTATUS.FIXED;
                r.UpdatedAt = DateTime.Now;
                r.UpdatedBy = "Admin";
                await roomRepository.SaveRoom(r);
            }
            else
            {
                throw new Exception("This room is already fixed!");
            }
        }
    }
}
