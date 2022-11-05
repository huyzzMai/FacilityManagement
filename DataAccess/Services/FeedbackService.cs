using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessObject.Commons;
using BusinessObject.Models;
using BusinessObject.RequestModel.FeedbackRequest;
using BusinessObject.ResponseModel.FeedbackResponse;
using DataAccess.IRepositories;
using DataAccess.IServices;

namespace DataAccess.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly ILogRepository _logRepository;
        private readonly IDeviceRepository _deviceRepository;

        public FeedbackService(IFeedbackRepository feedbackRepository, ILogRepository logRepository, IDeviceRepository deviceRepository)
        {
            _feedbackRepository = feedbackRepository;
            _logRepository = logRepository;
            _deviceRepository = deviceRepository;
        }

        async Task IFeedbackService.UpdateFeedback(int id, FeedbackUpdateRequest feedbackRequest)
        {
            try
            {
                Feedback feedback = await _feedbackRepository.GetFeedback(id);

                feedback.RoomId = feedbackRequest.roomId ??= feedback.RoomId;
                feedback.Flag = feedbackRequest.flag ??= feedback.Flag;
                feedback.DeviceId = feedbackRequest.deviceId ??= feedback.DeviceId;
                feedback.Description = feedbackRequest.description ??= feedback.Description;
                feedback.Image = feedbackRequest.image ??= feedback.Image;
                feedback.Status = feedbackRequest.status ??= feedback.Status;

                if (feedback.Equals(await _feedbackRepository.GetFeedback(id)))
                {
                    //update if changed
                    feedback.UpdatedAt = DateTime.Today;
                    feedback.UpdatedBy = "system";

                    await _feedbackRepository.Update(feedback);


                    //create log
                    Log log = new()
                    {
                        FeedbackId = feedback.Id,
                        DeviceId = feedback.DeviceId,
                        Status = feedback.Status,
                        Description = feedback.Description,
                        IsDeleted = false,
                        CreatedAt = DateTime.Now,
                        CreatedBy = "system",
                    };
                    await _logRepository.Create(log);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        async Task<int> IFeedbackService.CreateFeedback(FeedbackRequest feedbackRequest)
        {
            //try
            //{
                Feedback feedback = new()
                {
                    UserId = feedbackRequest.userId,
                    RoomId = feedbackRequest.roomId,
                    DeviceId = feedbackRequest.deviceId,
                    Description = feedbackRequest.description,
                    Image = feedbackRequest.image,
                    Status = CommonEnums.FEEDBACKSTATUS.PENDING,
                    IsDeleted = false,
                    CreatedAt = DateTime.Today,
                    CreatedBy = feedbackRequest.userId.ToString(),

                };
                await _feedbackRepository.Create(feedback);

                //create log
                Log log = new()
                {
                    FeedbackId = feedback.Id,
                    DeviceId = feedback.DeviceId,
                    Status = CommonEnums.LOGSTATUS.FEEDBACK_CREATE, 
                    Description = feedback.Description,
                    IsDeleted = false,
                    CreatedAt = DateTime.Now,
                    CreatedBy = "system",
                };
                await _logRepository.Create(log);

                return feedback.Id;
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception(ex.Message);
            //}
        }

        async Task<IEnumerable<FeedbackResponse>> IFeedbackService.GetAllFeedback()
        {
            try
            {
                IEnumerable<Feedback> feedbacks = await _feedbackRepository.GetList();
                IEnumerable <FeedbackResponse> FeedbackResponses = feedbacks
                    .Select(f => Feedback.MapToResponse(f));
                return FeedbackResponses;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        async Task<FeedbackResponse> IFeedbackService.GetFeedbackById(int id)
        {
            try
            {
                Feedback feedback = await _feedbackRepository.GetFeedback(id);
                return new FeedbackResponse() {
                    id = feedback.Id,
                    userName = feedback.User.FullName,
                    roomName = feedback.Room.Name,
                    deviceName = feedback.Device.Name, 
                    description = feedback.Description,
                    status = feedback.Status.ToString()
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        async Task IFeedbackService.DeleteFeedback(int id)
        {
            try
            {
                await _feedbackRepository.Delete(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task DenyFeedback(int id)
        {
            try
            {
                var feedback = await _feedbackRepository.GetFeedback(id);
                feedback.Status = CommonEnums.FEEDBACKSTATUS.DENY;
                await _feedbackRepository.Update(feedback);

                //create log
                Log log = new()
                {
                    FeedbackId = feedback.Id,
                    DeviceId = feedback.DeviceId,
                    Status = CommonEnums.LOGSTATUS.FEEDBACK_DENY,
                    Description = feedback.Description,
                    IsDeleted = false,
                    CreatedAt = DateTime.Now,
                    CreatedBy = "system",
                };
                await _logRepository.Create(log);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task AcceptFeedback(int id, int fixerId)
        {
            try
            {
                var feedback = await _feedbackRepository.GetFeedback(id);
                feedback.Status = CommonEnums.FEEDBACKSTATUS.ACCEPT;
                await _feedbackRepository.Update(feedback);

                //update device status
                var device = await _deviceRepository.GetDeviceAndDeleteIsFalse(feedback.DeviceId);
                device.Status = CommonEnums.DEVICESTATUS.INACTIVE;
                await _deviceRepository.UpdateDevice(device);

                //create log
                Log log = new()
                {
                    FeedbackId = feedback.Id,
                    DeviceId = feedback.DeviceId,
                    Status = CommonEnums.LOGSTATUS.FEEDBACK_ACCEPT,
                    Description = feedback.Description,
                    IsDeleted = false,
                    CreatedAt = DateTime.Now,
                    CreatedBy = "system",
                };
                await _logRepository.Create(log);
        }
            catch (Exception e)
            {
                throw new Exception(e.Message);
    }
}

        public async Task CloseFeedback(int id)
        {
            try
            {
                var feedback = await _feedbackRepository.GetFeedback(id);
                feedback.Status = CommonEnums.FEEDBACKSTATUS.CLOSE;
                await _feedbackRepository.Update(feedback);

                //update device status
                var device = await _deviceRepository.GetDeviceAndDeleteIsFalse(feedback.DeviceId);
                device.Status = CommonEnums.DEVICESTATUS.ACTIVE;
                await _deviceRepository.UpdateDevice(device);

                //create log
                Log log = new()
                {
                    FeedbackId = feedback.Id,
                    DeviceId = feedback.DeviceId,
                    Status = CommonEnums.LOGSTATUS.FEEDBACK_CLOSE,
                    Description = feedback.Description,
                    IsDeleted = false,
                    CreatedAt = DateTime.Now,
                    CreatedBy = "system",
                };
                await _logRepository.Create(log);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<IEnumerable<FeedbackResponse>> GetAllFeedbackByUserId(int userId)
        {
            var feedbacks = await _feedbackRepository.GetList();
            return feedbacks.Where(f => f.UserId.GetValueOrDefault().Equals(userId)).Select(f=>Feedback.MapToResponse(f));
        }
    }
}

