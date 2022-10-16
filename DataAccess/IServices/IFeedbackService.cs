using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessObject.Models;

namespace DataAccess.IServices
{
    public interface IFeedbackService
    {
        Task CreateFeedback(Feedback feedback);
        Task<IEnumerable<Feedback>> GetAllFeedback();
        Task<Feedback> GetFeedbackById(int id);
    }
}

