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
                    case CommonEnums.FEEDBACKSTATUS.CLOSE:
                        status = "CLOSE";
                        break;
                    case CommonEnums.FEEDBACKSTATUS.PENDING:
                        status = "PENDING";
                        break;
                    case CommonEnums.FEEDBACKSTATUS.DENY:
                        status = "DENY";
                        break;
                    case CommonEnums.FEEDBACKSTATUS.ACCEPT:
                        status = "ACCEPT";
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
        public async Task<IActionResult> PutFeedback(int id, FeedbackUpdateRequest feedback)
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
        public async Task<IActionResult> PostFeedback(FeedbackRequest feedbackRequest)
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

            return Ok();
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

        // PUT: api/Feedback/AcceptFeedback/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("AcceptFeedback/{id}")]
        public async Task<IActionResult> PutAcceptFeedback(int id)
        {
            try
            {
                await _service.AcceptFeedback(id);
            }
            catch (DbUpdateConcurrencyException)
            {

                throw;
            }

            return NoContent();
        }

        // PUT: api/Feedback/DenyFeedback/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("DenyFeedback/{id}")]
        public async Task<IActionResult> PutDenyFeedback(int id)
        {
            try
            {
                await _service.DenyFeedback(id);
            }
            catch (DbUpdateConcurrencyException)
            {

                throw;
            }

            return NoContent();
        }

        // PUT: api/Feedback/CloseFeedback/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("CloseFeedback/{id}")]
        public async Task<IActionResult> PutCloseFeedback(int id)
        {
            try
            {
                await _service.CloseFeedback(id);
            }
            catch (DbUpdateConcurrencyException)
            {

                throw;
            }

            return NoContent();
        }
    }
}
