using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.RequestModel.UserRequest
{
    public class UserRequest
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public byte[] Image { get; set; }
    }
}
