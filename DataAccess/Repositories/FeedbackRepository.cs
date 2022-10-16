using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessObject.Models;
using DataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class FeedbackRepository: IFeedbackRepository
    {
        private readonly FacilityFeedbackManagementContext _facilityFeedbackManagementContext;

        public FeedbackRepository(FacilityFeedbackManagementContext facilityFeedbackManagementContext)
        {
            _facilityFeedbackManagementContext = facilityFeedbackManagementContext;
        }

        public async Task Create(Feedback feedback)
        {
            Feedback _feedback = null;
            try
            {
                _feedback = await GetFeedback(feedback.Id);
                if (_feedback != null)
                {
                    throw new Exception("Create fail: " + "Id existed");
                }
                await _facilityFeedbackManagementContext.Feedbacks.AddAsync(feedback);
                await _facilityFeedbackManagementContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Create fail: "+ex.Message);
            }
        }

        public async Task Delete(int id)
        {
            Feedback feedback = null;
            try
            {
                feedback = await GetFeedback(id);
                if (feedback == null)
                {
                    throw new Exception("Delete fail: " + "Id not found");
                }
                _facilityFeedbackManagementContext.Feedbacks.Remove(feedback);
                await _facilityFeedbackManagementContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Delete fail: " + ex.Message);
            }
        }

        public async Task<Feedback> GetFeedback(int id)
        {
            Feedback feedback = null;
            try
            {
                feedback = await _facilityFeedbackManagementContext.Feedbacks.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Get fail: " + ex.Message);
            }

            return feedback;
        }

        public async Task<IEnumerable<Feedback>> GetList()
        {
            IEnumerable<Feedback> feedbacks = null;
            try
            {
                feedbacks = await _facilityFeedbackManagementContext.Feedbacks.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Get fail: " + ex.Message);
            }

            return feedbacks;
        }

        public async Task Update(Feedback _feedback)
        {
            Feedback feedback = null;
            try
            {
                feedback = await GetFeedback(_feedback.Id);
                if (feedback == null)
                {
                    throw new Exception("Update fail: " + "Id not found");
                }
                _facilityFeedbackManagementContext.Feedbacks.Update(feedback);
                _facilityFeedbackManagementContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Update fail: " + ex.Message);
            }
        }
    }
}

