using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataAccess.IServices;
using BusinessObject.ResponseModel.FeedbackResponse;
using BusinessObject.Commons;
using BusinessObject.RequestModel.FeedbackRequest;
using System.Diagnostics;

namespace FacilityManagement.Controllers.FeedbackController
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _service;

        public FeedbackController(IFeedbackService service)
        {
            _service = service;
        }

        // GET: api/Feedback
        [HttpGet]
        public async Task<IEnumerable<FeedbackResponse>> GetFeedbacks()
        {
            IEnumerable<FeedbackResponse> feedbacks = await _service.GetAllFeedback();

            feedbacks = feedbacks.Select(r =>
            {
                string status;

                switch (Int32.Parse(r.status))
                {
                    case CommonEnums.FEEDBACKSTATUS.FIXED:
                        status = "FIXED";
                        break;
                    case CommonEnums.FEEDBACKSTATUS.PENDING:
                        status = "PENDING";
                        break;
                    case CommonEnums.FEEDBACKSTATUS.DENY:
                        status = "DENY";
                        break;
                    default:
                        status = "UNDIFINED";
                        break;
                }
                r.status = status;
                return r;
            }).ToList();
            return feedbacks;
        }

        // GET: api/Feedback/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FeedbackResponse>> GetFeedback(int id)
        {
            var feedback = await _service.GetFeedbackById(id);

            if (feedback == null)
            {
                return NotFound();
            }

            return feedback;
        }

        // PUT: api/Feedback/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFeedback(int id, FeedbackRequest feedback)
        {
            try
            {
                await _service.UpdateFeedback(id, feedback);
            }
            catch (DbUpdateConcurrencyException)
            {
                
                    throw;
            }

            return NoContent();
        }

        // POST: api/Feedback
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<FeedbackRequest>> PostFeedback(FeedbackRequest feedbackRequest)
        {
            int response;
            try
            {
                response = await _service.CreateFeedback(feedbackRequest);
            }
            catch (DbUpdateException)
            {

                throw;
            }

            return CreatedAtAction("GetFeedback", new { id = response }, feedbackRequest);
        }

        // DELETE: api/Feedback/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFeedback(int id)
        {
            try
            {
                await _service.DeleteFeedback(id);
            }
            catch (DbUpdateException)
            {

                throw;
            }
            return CreatedAtAction("GetFeedbacks", "Feedback is deleted");
        }

        //private bool FeedbackExists(int id)
        //{
        //    return _service.Feedbacks.Any(e => e.Id == id);
        //}
    }
}
