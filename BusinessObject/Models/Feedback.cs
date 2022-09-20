using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObject.Models
{
    public partial class Feedback
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int DeviceTypeId { get; set; }
        public int RoomId { get; set; }
        public int? ResponseId { get; set; }
        public byte[] Image { get; set; }
        public bool? Flag { get; set; }
        public string Description { get; set; }
        public int? Status { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }

        public virtual DeviceType DeviceType { get; set; }
        public virtual Room Room { get; set; }
        public virtual User User { get; set; }
    }
}
