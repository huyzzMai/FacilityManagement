using BusinessObject.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace DataAccess.IRepositories
{
    public interface IDeviceRepository
    {
        Task<List<Device>> GetAllDevice();
        Task<Device> GetDeviceAndDeleteIsFalse(int id);
        Task<Device> GetDeviceByRoomID(int id);
        Task<Device> GetDeviceByDeviceTypeID(int id);
        Task<Device> GetDeviceByName(string name);
        Task SaveDevice(Device device);
        Task UpdateDevice(Device device);
        Device GetDeviceAndDeleteIsFalseNoTask(int id);
    }
}
