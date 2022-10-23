using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepositories
{
    public interface IDeviceTypeRepository
    {
        Task<List<DeviceType>> GetAllDeviceType();
        Task<DeviceType> GetDeviceTypeAndDeleteIsFalse(int id);
        Task<DeviceType> GetDeviceTypeByName(string name);
        Task SaveDeviceType(DeviceType deviceType);
        Task UpdateDeviceType(DeviceType deviceType);
    }
}
