using BusinessObject.Models;
using BusinessObject.RequestModel.DeviceTypeRequest;
using BusinessObject.ResponseModel.DeviceTypeResponse;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.IServices
{
    public interface IDeviceTypeService
    {
        Task<IEnumerable<DeviceTypeResponse>> GetAllDeviceType();
        Task<DeviceType> GetDeviceTypeById(int id);
        Task<DeviceType> GetDeviceTypeByName(string name);
        Task<DeviceTypeResponse> UpdateDeviceType(int id, DeviceTypeRequest deviceTypes);
        Task<DeviceTypeResponse> CreateDeviceType(DeviceTypeRequest deviceTypes);
        Task DeleteDeviceType(int id);
    }
}
