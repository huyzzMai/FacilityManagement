﻿using BusinessObject.Models;
using BusinessObject.RequestModel.DeviceRequest;
using BusinessObject.ResponseModel.DeviceResponse;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.IServices
{
    public interface IDeviceService
    {
        Task<IEnumerable<DeviceResponse>> GetAllDevice();
        Task<Device> GetDeviceById(int id);
        Task<Device> GetDeviceByName(string name);
        Task<DeviceResponse> UpdateDevice(int id, DeviceRequest devices);
        Task<DeviceResponse> CreateDevice(DeviceRequest devices);
        Task DeleteDevice(int id);
    }
}