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
using Microsoft.AspNetCore.Authorization;

namespace FacilityManagement.Controllers.FeedbackController
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Admin, User, Fixer")]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _service;

        public FeedbackController(IFeedbackService service)
        {
            _service = service;
        }
        
        // GET: api/Feedback/Feedbacks/5
        [HttpGet("User/{id}")]
        public async Task<IEnumerable<FeedbackResponse>> GetFeedbacksByUserId(int id)
        {
            var feedbacks = await _service.GetAllFeedbackByUserId(id);

            return feedbacks;
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

    }
}
