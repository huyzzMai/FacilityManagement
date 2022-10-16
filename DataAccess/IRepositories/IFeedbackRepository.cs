using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessObject.Models;

namespace DataAccess.IRepositories
{
    public interface IFeedbackRepository
    {
        public Task Create(Feedback feedback);
        public Task<Feedback> GetFeedback(int id);
        public Task<IEnumerable<Feedback>> GetList();
        public Task Update(Feedback feedback);
        public Task Delete(int id);
    }
}

