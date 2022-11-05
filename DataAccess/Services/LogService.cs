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
        private readonly ILogRepository logRepository;
        private readonly IUserRepository userRepository;

        public LogService(IUserRepository userRepository, ILogRepository logRepository)
        {
            this.userRepository = userRepository;
            this.logRepository= logRepository;
        }

        public async Task CreateLog(Log log)
        {
            try
            {
                await logRepository.Create(log);
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
                await logRepository.Delete(id);
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
                IEnumerable<Log> Logs = await logRepository.GetList();
                foreach(Log log in Logs)
                {
                    if (log.FixerId != null)
                    {
                        log.User = await userRepository.GetUserAndDeleteIsFalse(log.FixerId.GetValueOrDefault());
                    }
                }
                IEnumerable<LogResponse> LogResponses = Logs
                    .Select(l => Log.MapToResponse(l));
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
                Log log = await logRepository.GetLog(id);
                return Log.MapToResponse(log);
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
                Log _log = await logRepository.GetLog(id);

                await logRepository.Update(log);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
