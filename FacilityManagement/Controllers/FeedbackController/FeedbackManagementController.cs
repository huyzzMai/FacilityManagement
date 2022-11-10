using BusinessObject.RequestModel.FeedbackRequest;
using BusinessObject.ResponseModel.FeedbackResponse;
using DataAccess.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FacilityManagement.Controllers.FeedbackController
{
    [Route("api/feedback-management")]
    [ApiController]
    //[Authorize(Roles = "Admin")]
    public class FeedbackManagementController : ControllerBase
    {
        private readonly IFeedbackService _service;
        private readonly IUserService _userService;

        public FeedbackManagementController(IFeedbackService service, IUserService userService)
        {
            _service = service;
            _userService = userService;
        }

        // GET: api/Feedback
        [HttpGet]
        public async Task<IEnumerable<FeedbackResponse>> GetFeedbacks()
        {
            IEnumerable<FeedbackResponse> feedbacks = await _service.GetAllFeedback();

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

        // GET: api/Feedback/Pending
        [HttpGet("pending")]
        public async Task<IEnumerable<FeedbackResponse>> GetFeedbacksOnPending()
        {
            var feedbacks = await _service.GetAllFeedbackOnPending();

            return feedbacks;
        }

        [HttpGet("fixers")]
        public async Task<IActionResult> GetFixerList()
        {
            try
            {
                return Ok(await _userService.GetFixerList());
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

        // PUT: api/Feedback/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFeedback(int id, [FromBody] FeedbackUpdateRequest feedback)
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

        // PUT: api/Feedback/AcceptFeedback/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("accept-feedback/{id}")]
        public async Task<IActionResult> PutAcceptFeedback(int id, int fixerId)
        {
            try
            {
                await _service.AcceptFeedback(id, fixerId);
            }
            catch (DbUpdateConcurrencyException)
            {

                throw;
            }

            return NoContent();
        }

        // PUT: api/Feedback/DenyFeedback/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("deny-feedback/{id}")]
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
        //[HttpPut("CloseFeedback/{id}")]
        //public async Task<IActionResult> PutCloseFeedback(int id)
        //{
        //    try
        //    {
        //        await _service.CloseFeedback(id);
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {

        //        throw;
        //    }

        //    return NoContent();
        //}

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
    }
}
