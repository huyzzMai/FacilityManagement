using BusinessObject.Commons;
using BusinessObject.Models;
using DataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class LogRepository : ILogRepository
    {
        private readonly FacilityFeedbackManagementContext _facilityFeedbackManagementContext;

        public LogRepository(FacilityFeedbackManagementContext facilityFeedbackManagementContext)
        {
            _facilityFeedbackManagementContext = facilityFeedbackManagementContext;
        }
        public async Task Create(Log log)
        {
            try
            {
                Log _log = await GetLog(log.Id);
            if (_log != null)
                {
                    throw new Exception("Create fail: " + "Id existed");
                }
                await _facilityFeedbackManagementContext.Logs.AddAsync(log);
                await _facilityFeedbackManagementContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Create fail: " + ex.Message);
            }
        }

        public async Task Delete(int id)
        {
            Log log;
            try
            {
                log = await GetLog(id);
                if (log == null)
                {
                    throw new Exception("Delete fail: " + "Id not found");
                }
                log.IsDeleted = true;

                await Update(log);
            }
            catch (Exception ex)
            {
                throw new Exception("Delete fail: " + ex.Message);
            }
        }

        public async Task<Log> GetLog(int id)
        {
            Log log = null;
            try
            {
                log = await _facilityFeedbackManagementContext.Logs
                    .Where(f => f.IsDeleted == false && f.Id.Equals(id))
                    .Include(l => l.Device)
                    .Include(l => l.Feedback)
                    .Include(l => l.User)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Get fail: " + ex.Message);
            }

            return log;
        }

        public async Task<IEnumerable<Log>> GetList()
        {
            IEnumerable<Log> logs = null;
            try
            {
                logs = await _facilityFeedbackManagementContext.Logs
                    .Where(f => f.IsDeleted == false)
                    .Include("Feedback")
                    .Include("Device")
                    .AsNoTracking()
                    .ToListAsync();
                Debug.WriteLine("Get log List " + _facilityFeedbackManagementContext.Logs.Where(f => f.IsDeleted == false).Count());
            }
            catch (Exception ex)
            {
                throw new Exception("Get fail: " + ex.Message);
            }

            return logs;
        }

        public async Task<IEnumerable<Log>> GetLogsByFixerId(int id)
        {
            IEnumerable<Log> logs = await _facilityFeedbackManagementContext.Logs
                    .Where(f => f.FixerId == id)
                    .Include("Feedback")
                    .AsNoTracking().ToListAsync();
            return logs;
        }

        public async Task<Log> GetLogByFeedbackIdAndLogStatusIsAccept(int id)
        {
            Log log = await _facilityFeedbackManagementContext.Logs.SingleOrDefaultAsync(l => l.FeedbackId == id && l.Status==CommonEnums.LOGSTATUS.FEEDBACK_ACCEPT);
            return log;
        }

        public async Task Update(Log _log)
        {
            Log log;
            try
            {
                log = await GetLog(_log.Id);
                if (log == null)
                {
                    throw new Exception("Update fail: " + "Id not found");
                }
                _facilityFeedbackManagementContext.Logs.Update(log);
                _facilityFeedbackManagementContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Update fail: " + ex.Message);
            }
        }
    }
}
