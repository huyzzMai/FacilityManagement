using System;
using System.Collections.Generic;
using BusinessObject.Models;

namespace DataAccess.IRepositories
{
    public interface IFeedbackRepository
    {
        public void Create(Feedback feedback);
        public Feedback GetFeedback(int id);
        public IEnumerable<Feedback> GetList();
        public void Update(Feedback feedback);
        public void Delete(int id);
    }
}

