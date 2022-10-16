using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepositories
{
    public interface IRoomRepository
    {
        Task<List<Room>> GetAllRoom();
        Task<Room> GetRoomAndDeleteIsFalse(int id);
        Task<Room> GetRoomByName(string roomName);
        Task SaveRoom(Room room);
        Task DeleteRoom(Room room);
    }
}
