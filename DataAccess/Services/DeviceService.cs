using BusinessObject.Commons;
using BusinessObject.Models;
using BusinessObject.RequestModel.DeviceRequest;
using BusinessObject.ResponseModel.DeviceResponse;
using DataAccess.IRepositories;
using DataAccess.IServices;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Services
{
    public class DeviceService : IDeviceService
    {
        private readonly IDeviceRepository deviceRepository;

        public IConfiguration _configuration;
        public DeviceService(IDeviceRepository deviceRepository, IConfiguration configuration)
        {
            _configuration = configuration;
            this.deviceRepository = deviceRepository;
        }
        public async Task<Device> GetDeviceByName(string name)
        {
            Device r = await deviceRepository.GetDeviceByName(name);
            return r;
        }
        public async Task<Device> GetDeviceById(int id)
        {
            Device r = await deviceRepository.GetDeviceAndDeleteIsFalse(id);
            return r;
        }

        public async Task DeleteDevice(int id)
        {
            Device r = await deviceRepository.GetDeviceAndDeleteIsFalse(id);

            if (r == null)
            {
                throw new Exception("This Device is unavailable to delete.");
            }

            r.IsDeleted = true;

            await deviceRepository.UpdateDevice(r);
        }

        public async Task<IEnumerable<DeviceResponse>> GetAllDevice()
        {
            var devices = await deviceRepository.GetAllDevice();

            IEnumerable<DeviceResponse> result = devices.Select(
                device =>
                {

                    return new DeviceResponse()
                    {

                        DeviceTypeID = device.DeviceTypeId,
                        RoomID = device.RoomId,
                        Name = device.Name,
                        Status = device.Status
                    };
                }
                )
                .ToList();
            return result;
        }
        public async Task<DeviceResponse> UpdateDevice(int id, DeviceRequest devices)
        {
            var r = await deviceRepository.GetDeviceAndDeleteIsFalse(id);
            var u = await deviceRepository.GetDeviceByName(devices.Name);

            if (r == null)
            {
                throw new Exception("This Device cannot be updated because there is no room with that id!");
            }
            if (u == null)
            {
                r.DeviceTypeId = devices.DeviceTypeID;
                r.RoomId = devices.RoomID;
                r.Name = devices.Name;
                r.Status = devices.Status;
                r.UpdatedAt = DateTime.Now;

                await deviceRepository.UpdateDevice(r);

                var updevice = new DeviceResponse()
                {
                    Name = r.Name,
                };

                return updevice;
            }
            else
            {
                throw new Exception("This Device already existed!");
            }
        }

        public async Task<DeviceResponse> CreateDevice(DeviceRequest devices)
        {
            var r = await deviceRepository.GetDeviceByName(devices.Name);

            if (r == null)
            {
                Device rms = new Device();
                rms.DeviceTypeId = devices.DeviceTypeID;
                rms.RoomId = devices.RoomID;
                rms.Name = devices.Name;
                rms.Status = devices.Status;
                rms.IsDeleted = false;
                rms.CreatedAt = DateTime.Now;
                await deviceRepository.SaveDevice(rms);

                var updevice = new DeviceResponse()
                {
                    DeviceTypeID = rms.DeviceTypeId,
                    RoomID = rms.RoomId,
                    Name = rms.Name,
                    Status = rms.Status
                };

                return updevice;
            }
            else
            {
                throw new Exception("This room existed!");
            }

        }

        public async Task AddFixingStatus(int id)
        {
            Device r = await deviceRepository.GetDeviceAndDeleteIsFalse(id);
            if (r == null)
            {
                throw new Exception("This device is fixing .");
            }

            if (r.Status != CommonEnums.DEVICESTATUS.FIXING)
            {
                r.Status = CommonEnums.DEVICESTATUS.FIXING;
                r.UpdatedAt = DateTime.Now;
                r.UpdatedBy = "Admin";
                await deviceRepository.UpdateDevice(r);
            }
            else
            {
                throw new Exception("This device is fixing !");
            }
        }

        public async Task AddFixedStatus(int id)
        {
            Device r = await deviceRepository.GetDeviceAndDeleteIsFalse(id);
            if (r == null)
            {
                throw new Exception("This device is fixed .");
            }

            if (r.Status != CommonEnums.DEVICESTATUS.FIXED)
            {
                r.Status = CommonEnums.DEVICESTATUS.FIXED;
                r.UpdatedAt = DateTime.Now;
                r.UpdatedBy = "Admin";
                await deviceRepository.UpdateDevice(r);
            }
            else
            {
                throw new Exception("This device is fixed !");
            }
        }
    }
}
