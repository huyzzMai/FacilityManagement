using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepositories
{
    public interface ILogRepository
    {
        public Task Create(Log log);
        public Task<Log> GetLog(int id);
        public Task<Log> GetLogByFeedbackIdAndLogStatusIsAccept(int id);
        public Task<IEnumerable<Log>> GetList();
        Task<IEnumerable<Log>> GetLogsByFixerId(int id);
        public Task Update(Log log);
        public Task Delete(int id);
    }
}
