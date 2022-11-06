using BusinessObject.Commons;
using BusinessObject.ResponseModel.FeedbackResponse;
using BusinessObject.ResponseModel.LogResponse;
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
        public int? FixerId { get; set; }
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

        static public LogResponse MapToResponse(Log log)
        {
            LogResponse logResponse = new()
            {
                id = log.Id,
                feedbackDescription = log.Feedback.Description,
                deviceName = log.Device.Name,
                fixerName = log.User != null ? log.User.FullName : "Not Assined",
                logDescription = log.Description,
            };
            string status;

            switch (log.Status.GetValueOrDefault())
            {
                case CommonEnums.LOGSTATUS.FEEDBACK_CREATE:
                    status = "FEEDBACK_CREATE";
                    break;
                case CommonEnums.LOGSTATUS.FEEDBACK_ACCEPT:
                    status = "FEEDBACK_ACCEPT";
                    break;
                case CommonEnums.LOGSTATUS.FEEDBACK_DENY:
                    status = "FEEDBACK_DENY";
                    break;
                case CommonEnums.LOGSTATUS.FEEDBACK_CLOSE:
                    status = "FEEDBACK_CLOSE";
                    break;
                default:
                    status = "UNDIFINED";
                    break;
            }

            logResponse.status = status;

            return logResponse;
        }
    }
}
