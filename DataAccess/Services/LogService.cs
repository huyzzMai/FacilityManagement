using BusinessObject.Commons;
using BusinessObject.Models;
using BusinessObject.ResponseModel.LogResponse;
using DataAccess.IRepositories;
using DataAccess.IServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public async Task<LogStatisticResponse> GetLogStatisticCurrentMonth()
        {
            int currentMonth;
            int NumberOfCreatedFeedback;
            int NumberOfAcceptedFeedback;
            int NumberOfClosedFeedback;
            int NumberOfDeniedFeedback;
            float FinishedRatio;
            string mvpFixerName;

            try
            {
                DateTime today = DateTime.Today;
                currentMonth = today.Month;

                var logs = await logRepository.GetList();

                NumberOfCreatedFeedback = logs.Where(l => l.Status == CommonEnums.LOGSTATUS.FEEDBACK_CREATE 
                    && l.CreatedAt.GetValueOrDefault().Month == today.Month && l.CreatedAt.GetValueOrDefault().Year == today.Year).Count();
                NumberOfClosedFeedback = logs.Where(l => l.Status == CommonEnums.LOGSTATUS.FEEDBACK_CLOSE
                    && l.CreatedAt.GetValueOrDefault().Month == today.Month && l.CreatedAt.GetValueOrDefault().Year == today.Year).Count();
                NumberOfDeniedFeedback = logs.Where(l => l.Status == CommonEnums.LOGSTATUS.FEEDBACK_DENY
                    && l.CreatedAt.GetValueOrDefault().Month == today.Month && l.CreatedAt.GetValueOrDefault().Year == today.Year).Count();
                NumberOfAcceptedFeedback = logs.Where(l => l.Status == CommonEnums.LOGSTATUS.FEEDBACK_ACCEPT
                    && l.CreatedAt.GetValueOrDefault().Month == today.Month && l.CreatedAt.GetValueOrDefault().Year == today.Year).Count();
                FinishedRatio = NumberOfAcceptedFeedback != 0 ? NumberOfClosedFeedback / NumberOfAcceptedFeedback * 100 : 0;
                var mvpFixerId = logs.GroupBy(l => l.FixerId).Select(l => new { l.Key, Count = l.Count() })
                    .OrderByDescending(l => l.Count).ThenBy(l => l.Key).FirstOrDefault().Key;
                var mvpFixer = await userRepository.GetUserAndDeleteIsFalse(mvpFixerId.GetValueOrDefault());
                mvpFixerName = mvpFixer.FullName;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return new()
            {
                currentMonth = currentMonth,
                NumberOfCreatedFeedback = NumberOfCreatedFeedback,
                NumberOfAcceptedFeedback = NumberOfAcceptedFeedback,
                NumberOfClosedFeedback = NumberOfClosedFeedback,
                NumberOfDeniedFeedback = NumberOfDeniedFeedback,
                FinishedRatio = FinishedRatio,
                mvpFixerName = mvpFixerName
            };
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
