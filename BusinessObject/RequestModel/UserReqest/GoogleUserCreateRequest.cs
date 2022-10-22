using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.RequestModel.UserReqest
{
    public class GoogleUserCreateRequest
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Img { get; set; }
    }
}
