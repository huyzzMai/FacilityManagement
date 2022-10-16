﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.ResponseModel.UserResponse
{
    public class UserResponse
    {
        public string FullName { get; set; } 
        public string Email { get; set; }
        public string Image { get; set; }
        public string Role { get; set; }
        public int? Status { get; set; }
        
    }
}