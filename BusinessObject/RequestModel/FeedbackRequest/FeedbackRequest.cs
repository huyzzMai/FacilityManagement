using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.RequestModel.FeedbackRequest
{
    public class FeedbackRequest
    {
        public int deviceId;

        public byte[] image;

        public string description;
    }
}
