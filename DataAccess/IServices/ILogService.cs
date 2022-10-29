using BusinessObject.Models;
using BusinessObject.ResponseModel.LogResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IServices
{
    public interface ILogService
    {
        Task CreateLog(Log log);
        Task UpdateLog(int id, Log log);
        Task DeleteLog(int id);
        Task<IEnumerable<LogResponse>> GetAllLog();
        Task<LogResponse> GetLogById(int id);
    }
}
