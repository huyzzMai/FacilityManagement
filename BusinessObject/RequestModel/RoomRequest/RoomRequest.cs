using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.RequestModel.RoomRequest
{
    public class RoomRequest
    {
        public string Name { get; set; }
        public string Level { get; set; }
        public int? Status { get; set; }
    }
}
