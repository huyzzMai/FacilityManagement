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
using Microsoft.AspNetCore.Http;

namespace FacilityManagement.Controllers.FeedbackController
{
    [Route("api/feedback")]
    [ApiController]
    [Authorize(Roles = "Admin,User,Fixer")]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _service;
        private readonly IUserService userService;

        public FeedbackController(IFeedbackService service, IUserService userService)
        {
            _service = service;
            this.userService = userService;
        }

        // GET: api/Feedback/Feedbacks/5
        [HttpGet]
        public async Task<IEnumerable<FeedbackResponse>> GetFeedbacksByUserId()
        {
            // Get id of current log in user 
            int userId = userService.GetCurrentLoginUserId(Request.Headers["Authorization"]);

            var feedbacks = await _service.GetAllFeedbackByUserId(userId);

            return feedbacks;
        }

        // POST: api/Feedback
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<IActionResult> PostFeedback([FromBody] FeedbackRequest feedbackRequest)
        //{
        //    int response;
        //    try
        //    {
        //        // Get id of current log in user 
        //        int userId = userService.GetCurrentLoginUserId(Request.Headers["Authorization"]);

        //        response = await _service.CreateFeedback(feedbackRequest, userId);
        //    }
        //    catch (Exception ex)
        //    {

        //        return StatusCode(StatusCodes.Status500InternalServerError,
        //            ex.Message);
        //    }

        //    return Ok();
        //}

        [HttpPost]
        public async Task<IActionResult> CreateFeedback([FromBody] FeedbackRequest feedbackRequest)
        {
            int response;
            try
            {
                // Get id of current log in user 
                int userId = userService.GetCurrentLoginUserId(Request.Headers["Authorization"]);

                response = await _service.CreateFeedback(feedbackRequest, userId);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }

            return Ok();
        }
    }
}
