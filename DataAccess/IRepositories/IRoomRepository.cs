using BusinessObject.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.IRepositories
{
    public interface IRoomRepository
    {
        Task<List<Room>> GetAllRoom();
        Task<Room> GetRoomAndDeleteIsFalse(int id);
        Task<Room> GetRoomByName(string roomName);
        Task SaveRoom(Room room);
        Task UpdateRoom(Room room);
        Room GetRoomAndDeleteIsFalseNoTask(int id);
    }
}
