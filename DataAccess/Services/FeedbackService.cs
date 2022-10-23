using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public FeedbackService(IFeedbackRepository feedbackRepository)
        {
            _feedbackRepository = feedbackRepository;
        }

        async Task IFeedbackService.UpdateFeedback(int id, FeedbackUpdateRequest feedbackRequest)
        {
            try
            {
                Feedback feedback = await _feedbackRepository.GetFeedback(id);

                feedback.RoomId = feedbackRequest.roomId ??= feedback.RoomId;
                feedback.UserId = feedbackRequest.userId ??= feedback.UserId;
                feedback.Flag = feedbackRequest.flag ??= feedback.Flag;
                feedback.DeviceId = feedbackRequest.deviceId ??= feedback.DeviceId;
                feedback.Description = feedbackRequest.description ??= feedback.Description;
                feedback.Image = feedbackRequest.image ??= feedback.Image;
                feedback.Status = feedbackRequest.status ??= feedback.Status;

                feedback.UpdatedAt = DateTime.Today;
                feedback.UpdatedBy = "system";

                await _feedbackRepository.Update(feedback);
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
                Feedback feedback = new Feedback()
                {
                    UserId = feedbackRequest.userId,
                    RoomId = feedbackRequest.roomId,
                    DeviceId = feedbackRequest.deviceId,
                    Description = feedbackRequest.description,
                    Image = feedbackRequest.image,
                    Status = 1,
                    IsDeleted = false,
                    CreatedAt = DateTime.Today,
                    CreatedBy = feedbackRequest.userId.ToString(),

                };
                await _feedbackRepository.Create(feedback);

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
                    .Select(f => new FeedbackResponse() { id = f.Id, userName = f.User.FullName, roomName = f.Room.Name, deviceName = f.Device.Name, description = f.Description, status = f.Status.ToString() });
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
    }
}

