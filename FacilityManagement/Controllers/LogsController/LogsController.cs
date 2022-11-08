using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Models;
using DataAccess.IServices;
using BusinessObject.ResponseModel.LogResponse;

namespace FacilityManagement.Controllers.LogsController
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly ILogService _service;

        public LogsController(ILogService service)
        {
            _service = service;
        }

        // GET: api/Logs
        [HttpGet]
        public async Task<IEnumerable<LogResponse>> GetLogs()
        {
            return await _service.GetAllLog();
        }

        // GET: api/Logs/Statistic
        [HttpGet("Statistic")]
        public async Task<LogStatisticResponse> GetStatistic()
        {
            return await _service.GetLogStatisticCurrentMonth();
        }

        // GET: api/Logs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LogResponse>> GetLog(int id)
        {
            var log = await _service.GetLogById(id);

            if (log == null)
            {
                return NotFound();
            }

            return log;
        }

        // PUT: api/Logs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLog(int id, Log log)
        {
            if (id != log.Id)
            {
                return BadRequest();
            }
            
                await _service.UpdateLog(id, log);

            return NoContent();
        }

        // POST: api/Logs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Log>> PostLog(Log log)
        {
            await _service.CreateLog(log);

            return CreatedAtAction("GetLog", new { id = log.Id }, log);
        }

        // DELETE: api/Logs/5
        [HttpDelete("{id}")]
        public async Task DeleteLog(int id)
        {
            await _service.DeleteLog(id);
        }
    }
}
