using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObject.Models
{
    public partial class Device
    {
        public int Id { get; set; }
        public int DeviceTypeId { get; set; }
        public int? RoomId { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }

        public virtual DeviceType DeviceType { get; set; }
        public virtual Room Room { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; set; }
    }
}
