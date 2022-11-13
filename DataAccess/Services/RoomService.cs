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
using System.Xml.Linq;

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
                    else if (room.Status == CommonEnums.ROOMSTATUS.INACTIVE)
                    {
                        status = "InActive";
                    } 
                    else
                    {
                        status = "Occupied";
                    }

                    return new RoomResponse()
                    {   
                        id = room.Id,
                        Name = room.Name,
                        Level = room.Level,
                        Status = status
                    };
                }
                )
                .ToList();
            return result;
        }
        public async Task UpdateRoom(int id, RoomRequest rooms)
        {
            var r = await roomRepository.GetRoomAndDeleteIsFalse(id);
            var u = await roomRepository.GetRoomByName(rooms.Name);

            if (r == null)
            {
                throw new Exception("This room cannot be updated because there is no room with that id!");
            }
            if (u == null)
            {
                if (rooms.Name == null)
                {
                    r.Name = r.Name;
                }
                else
                {
                    r.Name = rooms.Name;
                }
                if (rooms.Level == null)
                {
                    r.Level = r.Level;
                }
                else
                {
                    r.Level = rooms.Level;
                }
                
                //r.Status = rooms.Status;
                r.UpdatedAt = DateTime.Now;
                r.UpdatedBy = "Admin";

                await roomRepository.UpdateRoom(r);

                //var uproom = new RoomResponse()
                //{
                //    Name = r.Name,
                //    Level = r.Level,
                //    Status = ""
                //};

                //return uproom;
            }
            else
            {
                throw new Exception("Another room already existed with this name, please try again!");
            }
        }

        public async Task UpdateRoomStatusInActive(int id)
        {
            var r = await roomRepository.GetRoomAndDeleteIsFalse(id);

            if (r == null)
            {
                throw new Exception("This room cannot be updated because there is no room with that id!");
            }
            if (r.Status == CommonEnums.ROOMSTATUS.INACTIVE)
            {
                throw new Exception("This room is already inactive");
            }
            r.Status = CommonEnums.ROOMSTATUS.INACTIVE;
                r.UpdatedAt = DateTime.Now;
                r.UpdatedBy = "Admin";

                await roomRepository.UpdateRoom(r);
           
        }

        public async Task RemoveRoomStatusInActive(int id)
        {
            var r = await roomRepository.GetRoomAndDeleteIsFalse(id);

            if (r == null)
            {
                throw new Exception("This room cannot be updated because there is no room with that id!");
            }
            if (r.Status != CommonEnums.ROOMSTATUS.INACTIVE)
            {
                throw new Exception("This room is already active");
            }
                r.Status = CommonEnums.ROOMSTATUS.ACTIVE;
                r.UpdatedAt = DateTime.Now;
                r.UpdatedBy = "Admin";

                await roomRepository.UpdateRoom(r);
            

        }

        public async Task<RoomResponse> CreateRoom(RoomRequest rooms)
        {
            var r = await roomRepository.GetRoomByName(rooms.Name);

            if (r == null)
            {
                Room rms = new Room();
                rms.Name = rooms.Name;
                rms.Level = rooms.Level;
                rms.Status = CommonEnums.ROOMSTATUS.ACTIVE;
                rms.IsDeleted = false;
                rms.CreatedAt = DateTime.Now;
                rms.CreatedBy = "Admin";

                await roomRepository.SaveRoom(rms);

                var uproom = new RoomResponse()
                {
                    Name = rms.Name,
                    Level = rms.Level,
                    Status = "Active"
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
    }
}
