using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.RequestModel.UserRequest
{
    public class EmployeeCreateRequest
    {
        public string Email { get; set; }
        public int Role { get; set; }
    }
}
