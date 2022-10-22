using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.RequestModel.FeedbackRequest
{
    public class FeedbackRequest
    {
        public int id;
        public int userId;
        public int deviceId;
        public int roomId;
        public byte[] image;
        public string description;
    }
}
