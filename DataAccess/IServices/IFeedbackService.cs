using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessObject.Models;
using BusinessObject.RequestModel.FeedbackRequest;
using BusinessObject.ResponseModel.FeedbackResponse;

namespace DataAccess.IServices
{
    public interface IFeedbackService
    {
        Task<int> CreateFeedback(FeedbackRequest feedbackRequest);
        Task UpdateFeedback(int id, FeedbackUpdateRequest feedbackRequest);
        Task DeleteFeedback(int id);
        Task<IEnumerable<FeedbackResponse>> GetAllFeedback();
        Task<FeedbackResponse> GetFeedbackById(int id);
        Task DenyFeedback(int id);
        Task AcceptFeedback(int id, int fixerId);
        Task CloseFeedback(int id);
        Task<IEnumerable<FeedbackResponse>> GetAllFeedbackByUserId(int userId);
    }
}

