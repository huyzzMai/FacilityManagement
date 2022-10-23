using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.RequestModel.FeedbackRequest
{
    public class FeedbackUpdateRequest
    {
        public int? userId;
        public int? deviceId;
        public int? roomId;
        public byte[] image;
        public string description;
        public bool? flag;
        public int? status;
    }
}
