using BusinessObject.Commons;
using BusinessObject.ResponseModel.FeedbackResponse;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace BusinessObject.Models
{
    public partial class Feedback
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int DeviceId { get; set; }
        public int RoomId { get; set; }
        public byte[] Image { get; set; }
        public bool? Flag { get; set; }
        public string Description { get; set; }
        public int? Status { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }

        public virtual Device Device { get; set; }
        public virtual Room Room { get; set; }
        public virtual User User { get; set; }

        static public FeedbackResponse MapToResponse(Feedback feedback)
        {
            FeedbackResponse feedbackResponse = new()
            {
                id = feedback.Id,
                userName = feedback.User.FullName,
                email = feedback.User.Email,
                roomName = feedback.Room.Name,
                deviceName = feedback.Device.Name,
                description = feedback.Description,
            };
            string status;

            switch (feedback.Status.GetValueOrDefault())
            {
                case CommonEnums.FEEDBACKSTATUS.CLOSE:
                    status = "CLOSE";
                    break;
                case CommonEnums.FEEDBACKSTATUS.PENDING:
                    status = "PENDING";
                    break;
                case CommonEnums.FEEDBACKSTATUS.DENY:
                    status = "DENY";
                    break;
                case CommonEnums.FEEDBACKSTATUS.ACCEPT:
                    status = "ACCEPT";
                    break;
                default:
                    status = "UNDIFINED";
                    break;
            }

            feedbackResponse.status = status;

            return feedbackResponse;
        }
    }
}
