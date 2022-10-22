using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.ResponseModel.FeedbackResponse
{
    public class FeedbackResponse
    {
        public int id { get; set; }
        public string userName { get; set; }
        public string roomName { get; set; }
        public string deviceName { get; set; }
        public string description { get; set; }
        public string status { get; set; }


    }
}
