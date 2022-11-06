using BusinessObject.Commons;
using BusinessObject.Models;
using DataAccess.IRepositories;
using DataAccess.IServices;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class DeviceRepository : IDeviceRepository
    {
        private readonly FacilityFeedbackManagementContext dbContext;
        public DeviceRepository(FacilityFeedbackManagementContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<Device>> GetAllDevice()
        {
            List<Device> devices = await dbContext.Devices.Where(r => r.IsDeleted == false).ToListAsync();
            return devices;
        }
        public async Task<Device> GetDeviceByName(string name)
        {
            Device rm = await dbContext.Devices.Where(r => r.Name == name && r.IsDeleted == false).FirstOrDefaultAsync();
            return rm;
        }
        public async Task<Device> GetDeviceAndDeleteIsFalse(int id)
        {
            Device rm = await dbContext.Devices.Where(r => r.Id.Equals(id) && r.IsDeleted == false)
                .FirstOrDefaultAsync();
            return rm;
        }
        public async Task SaveDevice(Device devices)
        {
            await dbContext.Devices.AddAsync(devices);
            await dbContext.SaveChangesAsync();
        }
        public async Task UpdateDevice(Device devices)
        {
            dbContext.Devices.Update(devices);
            await dbContext.SaveChangesAsync();
        }
    }
}
