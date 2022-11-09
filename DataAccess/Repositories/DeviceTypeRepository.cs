using BusinessObject.Models;
using DataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class DeviceTypeRepository : IDeviceTypeRepository
    {
        private readonly FacilityFeedbackManagementContext dbContext;
        public DeviceTypeRepository(FacilityFeedbackManagementContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<DeviceType>> GetAllDeviceType()
        {
            List<DeviceType> devices = await dbContext.DeviceTypes.Where(d => d.IsDeleted == false).ToListAsync();
            return devices;
        }
        public async Task<DeviceType> GetDeviceTypeByName(string name)
        {
            DeviceType rm = await dbContext.DeviceTypes.Where(r => r.Name == name && r.IsDeleted == false).FirstOrDefaultAsync();
            return rm;
        }
        public async Task<DeviceType> GetDeviceTypeAndDeleteIsFalse(int id)
        {
            DeviceType rm = await dbContext.DeviceTypes.Where(r => r.Id.Equals(id) && r.IsDeleted == false)
                .FirstOrDefaultAsync();
            return rm;
        }
        public async Task SaveDeviceType(DeviceType deviceTypes)
        {
                await dbContext.DeviceTypes.AddAsync(deviceTypes);
                await dbContext.SaveChangesAsync();
        }
        public async Task UpdateDeviceType(DeviceType deviceTypes)
        {
            dbContext.DeviceTypes.Update(deviceTypes);
            await dbContext.SaveChangesAsync();
        }
    }
}
