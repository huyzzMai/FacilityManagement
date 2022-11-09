using BusinessObject.Models;
using DataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly FacilityFeedbackManagementContext dbContext;
        public RoomRepository(FacilityFeedbackManagementContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<Room>> GetAllRoom()
        {
            List<Room> rooms = await dbContext.Rooms.Where(p => p.IsDeleted == false).ToListAsync();
            return rooms;
        }
        public async Task<Room> GetRoomByName(string name)
        {
            Room rm = await dbContext.Rooms.Where(r => r.Name == name && r.IsDeleted == false).FirstOrDefaultAsync();
            return rm;
        }
        public async Task<Room> GetRoomAndDeleteIsFalse(int id)
        {
            Room rm = await dbContext.Rooms.Where(r => r.Id.Equals(id) && r.IsDeleted == false)
                .FirstOrDefaultAsync();
            return rm;
        }
        public async Task SaveRoom(Room room)
        {
            try
            {
                await dbContext.Rooms.AddAsync(room);
                await dbContext.SaveChangesAsync();
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task UpdateRoom(Room room)
        {
            dbContext.Rooms.Update(room);
            await dbContext.SaveChangesAsync();
        }
    }
}
