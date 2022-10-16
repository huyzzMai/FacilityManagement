using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessObject.Models;
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

        async Task IFeedbackService.CreateFeedback(Feedback feedback)
        {
            try
            {
                await _feedbackRepository.Create(feedback);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        async Task<IEnumerable<Feedback>> IFeedbackService.GetAllFeedback()
        {
            try
            {
                return await _feedbackRepository.GetList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        async Task<Feedback> IFeedbackService.GetFeedbackById(int id)
        {
            try
            {
                return await _feedbackRepository.GetFeedback(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

