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
        //Task<RoomResponse> UpdateRoom(int id, RoomRequest rooms);
        Task UpdateRoom(int id, RoomRequest rooms);
        Task UpdateRoomStatusInActive(int id);
        Task RemoveRoomStatusInActive(int id);
        Task<RoomResponse> CreateRoom(RoomRequest rooms);
        Task DeleteRoom(int id);
    }
}
