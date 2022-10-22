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
        Task CreateFeedback(FeedbackRequest feedbackRequest);
        Task UpdateFeedback(int id, FeedbackRequest feedbackRequest);
        Task DeleteFeedback(int id);
        Task<IEnumerable<FeedbackResponse>> GetAllFeedback();
        Task<FeedbackResponse> GetFeedbackById(int id);
    }
}

