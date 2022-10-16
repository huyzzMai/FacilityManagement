using BusinessObject.Models;
using BusinessObject.RequestModel.RoomRequest;
using BusinessObject.ResponseModel.RoomResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IServices
{
    public interface IRoomService
    {
        Task<IEnumerable<RoomResponse>> GetAllRoom();
        Task<Room> GetRoomById(int id);
        Task<Room> GetRoomByName(string name);
        Task<RoomResponse> UpdateRoom(string name, RoomRequest rooms);
        Task<RoomResponse> CreateRoom(string naem, RoomRequest rooms);
        Task DeleteRoom(string name);
    }
}
