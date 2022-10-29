using BusinessObject.Models;
using BusinessObject.ResponseModel.LogResponse;
using DataAccess.IRepositories;
using DataAccess.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Services
{
    public class LogService : ILogService
    {
        private readonly ILogRepository _LogRepository;

        public LogService(ILogRepository LogRepository)
        {
            _LogRepository = LogRepository;
        }
        public async Task CreateLog(Log log)
        {
            try
            {
                await _LogRepository.Create(log);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteLog(int id)
        {
            try
            {
                await _LogRepository.Delete(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<LogResponse>> GetAllLog()
        {
            try
            {
                IEnumerable<Log> Logs = await _LogRepository.GetList();
                IEnumerable<LogResponse> LogResponses = Logs
                    .Select(l => new LogResponse() { id = l.Id, feedbackDescription = l.Feedback.Description, status = l.Status.ToString(), logDescription = l.Description, deviceName = l.Device.Name});
                return LogResponses;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<LogResponse> GetLogById(int id)
        {
            try
            {
                Log log = await _LogRepository.GetLog(id);
                return new LogResponse()
                {
                    id = log.Id,
                    feedbackDescription = log.Feedback.Description,
                    deviceName = log.Device.Name,
                    logDescription = log.Description,
                    status = log.Status.ToString(),
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateLog(int id, Log log)
        {
            try
            {
                Log _log = await _LogRepository.GetLog(id);

                await _LogRepository.Update(log);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
