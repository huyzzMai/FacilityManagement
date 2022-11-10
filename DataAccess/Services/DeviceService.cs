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
                    string status = null;
                    if (device.Status == CommonEnums.DEVICESTATUS.ACTIVE)
                    {
                        status = "ACTIVE";
                    }
                    else if (device.Status == CommonEnums.DEVICESTATUS.INACTIVE)
                    {
                        status = "INACTIVE";
                    }
                    return new DeviceResponse()
                    {   
                        id = device.Id,
                        DeviceTypeID = device.DeviceTypeId,
                        RoomID = device.RoomId,
                        Name = device.Name,
                        Status = status
                    };
                }
                )
                .ToList();
            return result;
        }
        public async Task UpdateDevice(int id, UpdateDeviceRequest devices)
        {
            var r = await deviceRepository.GetDeviceAndDeleteIsFalse(id);
            var u = await deviceRepository.GetDeviceByName(devices.Name);
            //var h = await deviceRepository.GetDeviceByRoomID(r.RoomId ?? default(int));
            //var g = await deviceRepository.GetDeviceByDeviceTypeID(devices.DeviceTypeID);

            if (r == null)
            {
                throw new Exception("This Device cannot be updated because there is no room with that id!");
            }
            if (u == null)
            {
                
                if (devices.Name == null)
                {
                    r.Name = r.Name;
                }
                else
                {
                    r.Name = devices.Name;
                }
                //r.DeviceTypeId = devices.DeviceTypeID;
                //r.RoomId = devices.RoomID;
                //r.Name = devices.Name;
                //r.Status = ;
                r.UpdatedAt = DateTime.Now;
                r.UpdatedBy = "Admin";

                await deviceRepository.UpdateDevice(r);

                //var updevice = new DeviceResponse()
                //{   
                //    DeviceTypeID = devices.DeviceTypeID,
                //    RoomID = devices.RoomID,
                //    Name = r.Name

                //};

                //return updevice;
            }
            else
            {
                throw new Exception("Another device already existed with this name" );
            }
        }

        public async Task CreateDevice(DeviceRequest devices)
        {
            var r = await deviceRepository.GetDeviceByName(devices.Name);
            var h = await deviceRepository.GetDeviceByRoomID(devices.RoomID);
            var g = await deviceRepository.GetDeviceByDeviceTypeID(devices.DeviceTypeID);

            if (r == null && h != null && g != null)
            {
                Device rms = new Device();
                rms.DeviceTypeId = devices.DeviceTypeID;
                rms.RoomId = devices.RoomID;
                rms.Name = devices.Name;
                rms.Status = 0;
                rms.IsDeleted = false;
                rms.CreatedAt = DateTime.Now;
                rms.CreatedBy = "Admin";
                await deviceRepository.SaveDevice(rms);

                //var updevice = new DeviceResponse()
                //{
                //    DeviceTypeID = rms.DeviceTypeId,
                //    RoomID = rms.RoomId,
                //    Name = rms.Name,
                //    Status = rms.Status
                //};
                //return updevice;
            }
            else
            {
                throw new Exception("This Device already exist or roomID, DeviceTypeID are not found, please try again!");
            }

        }

        
    }
}
