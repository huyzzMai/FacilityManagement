﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BusinessObject.ResponseModel.DeviceResponse
{
    public class DeviceResponse
    {   
        public int id { get; set; }
        public int DeviceTypeID { get; set; }
        public int? RoomID { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
    }
}
