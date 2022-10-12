using System;
using System.Collections.Generic;
using BusinessObject.Models;
using DataAccess.IRepositories;

namespace DataAccess.Repositories
{
    public class FeedbackRepository: IFeedbackRepository
    {
        private readonly FacilityFeedbackManagementContext _facilityFeedbackManagementContext;

        public FeedbackRepository(FacilityFeedbackManagementContext facilityFeedbackManagementContext)
        {
            _facilityFeedbackManagementContext = facilityFeedbackManagementContext;
        }

        public void Create(Feedback feedback)
        {
            Feedback _feedback = null;
            try
            {
                _feedback = GetFeedback(feedback.Id);
                if (_feedback != null)
                {
                    throw new Exception("Create fail: " + "Id existed");
                }
                _facilityFeedbackManagementContext.Feedbacks.Add(feedback);
                _facilityFeedbackManagementContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Create fail: "+ex.Message);
            }
        }

        public void Delete(int id)
        {
            Feedback feedback = null;
            try
            {
                feedback = GetFeedback(id);
                if (feedback == null)
                {
                    throw new Exception("Delete fail: " + "Id not found");
                }
                _facilityFeedbackManagementContext.Feedbacks.Remove(feedback);
                _facilityFeedbackManagementContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Delete fail: " + ex.Message);
            }
        }

        public Feedback GetFeedback(int id)
        {
            Feedback feedback = null;
            try
            {
                feedback = _facilityFeedbackManagementContext.Feedbacks.Find(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Get fail: " + ex.Message);
            }

            return feedback;
        }

        public IEnumerable<Feedback> GetList()
        {
            IEnumerable<Feedback> feedbacks = null;
            try
            {
                feedbacks = _facilityFeedbackManagementContext.Feedbacks.AsQueryable();
            }
            catch (Exception ex)
            {
                throw new Exception("Get fail: " + ex.Message);
            }

            return feedbacks;
        }

        public void Update(Feedback _feedback)
        {
            Feedback feedback = null;
            try
            {
                feedback = GetFeedback(_feedback.Id);
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

