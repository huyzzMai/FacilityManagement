using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.ResponseModel.UserResponse
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string FullName { get; set; } 
        public string Email { get; set; }
        public byte[] Image { get; set; }
        public string Role { get; set; }
        public string Status { get; set; }
        
    }
}
