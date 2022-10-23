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
        [Required]
        public int userId;

        [Required]
        public int deviceId;

        [Required]
        public int roomId;
        public byte[] image;

        [Required]
        public string description;
    }
}
