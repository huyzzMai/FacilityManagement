﻿using BusinessObject.Commons;
using BusinessObject.Models;
using BusinessObject.RequestModel.DeviceTypeRequest;
using BusinessObject.ResponseModel.DeviceTypeResponse;
using BusinessObject.ResponseModel.RoomResponse;
using DataAccess.IRepositories;
using DataAccess.IServices;
using DataAccess.Repositories;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Services
{
    public class DeviceTypeService : IDeviceTypeService
    {
        private readonly IDeviceTypeRepository deviceTypeRepository;

        public IConfiguration _configuration;
        public DeviceTypeService(IDeviceTypeRepository deviceTypeRepository, IConfiguration configuration)
        {
            _configuration = configuration;
            this.deviceTypeRepository = deviceTypeRepository;
        }
        public async Task<DeviceType> GetDeviceTypeByName(string name)
        {
            DeviceType r = await deviceTypeRepository.GetDeviceTypeByName(name);
            return r;
        }
        public async Task<DeviceType> GetDeviceTypeById(int id)
        {
            DeviceType r = await deviceTypeRepository.GetDeviceTypeAndDeleteIsFalse(id);
            return r;
        }
        
        public async Task DeleteDeviceType(int id)
        {
            DeviceType r = await deviceTypeRepository.GetDeviceTypeAndDeleteIsFalse(id);

            if (r == null)
            {
                throw new Exception("This Device Type is unavailable to delete.");
            }

            r.IsDeleted = true;

            await deviceTypeRepository.UpdateDeviceType(r);
        }

        public async Task<IEnumerable<DeviceTypeResponse>> GetAllDeviceType()
        {
            var devices = await deviceTypeRepository.GetAllDeviceType();

            IEnumerable<DeviceTypeResponse> result = devices.Select(
                device =>
                {
                    // Cast status from int to string for response
                    string status;
                    if (device.Status == CommonEnums.ROOMSTATUS.ACTIVE)
                    {
                        status = "Active";
                    }
                    else
                    {
                        status = "InActive";
                    }

                    return new DeviceTypeResponse()
                    {
                        Name = device.Name,
                        Status = status
                    };
                }
                )
                .ToList();
            return result;
        }
        public async Task<DeviceTypeResponse> UpdateDeviceType(int id, DeviceTypeRequest devices)
        {
            var r = await deviceTypeRepository.GetDeviceTypeAndDeleteIsFalse(id);
            var u = await deviceTypeRepository.GetDeviceTypeByName(devices.Name);

            if (r == null)
            {
                throw new Exception("This Device Type cannot be updated because there is no room with that id!");
            }
            if (u == null)
            {
                r.DepartmentId = devices.DepartmentId;
                r.Name = devices.Name;
                r.Status = devices.Status;

                await deviceTypeRepository.UpdateDeviceType(r);

                var updevice = new DeviceTypeResponse()
                {
                    Name = r.Name,
                };

                return updevice;
            }
            else
            {
                throw new Exception("This Device Type already existed!");
            }
        }

        public async Task<DeviceTypeResponse> CreateDeviceType(DeviceTypeRequest devices)
        {
            var r = await deviceTypeRepository.GetDeviceTypeByName(devices.Name);

            if (r == null)
            {
                DeviceType rms = new DeviceType();
                rms.DepartmentId= devices.DepartmentId;
                rms.Name = devices.Name;
                rms.Status = devices.Status;
                rms.IsDeleted = false;
                await deviceTypeRepository.SaveDeviceType(rms);

                var updevice = new DeviceTypeResponse()
                {
                    DepartmentId = rms.DepartmentId,
                    Name = rms.Name,
                };

                return updevice;
            }
            else
            {
                throw new Exception("This room existed!");
            }

        }
        
    }
}
