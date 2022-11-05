using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace BusinessObject.Models
{
    public partial class Log
    {
        public int Id { get; set; }
        public int FeedbackId { get; set; }
        public int DeviceId { get; set; }
        public int FixerId { get; set; }
        public int? Status { get; set; }
        public string Description { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }

        public virtual Device Device { get; set; }
        public virtual Feedback Feedback { get; set; }
        public virtual User User { get; set; }
    }
}
