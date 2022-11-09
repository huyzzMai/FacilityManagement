using DataAccess.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace FacilityManagement.Controllers.FeedbackController
{
    [Route("api/fixerfeedback")]
    [ApiController]
    [Authorize(Roles = "Fixer")]
    public class FixerFeedbackController : ControllerBase
    {
        private readonly IFeedbackService feedbackService;

        private readonly IUserService userService;

        public FixerFeedbackController(IFeedbackService feedbackService, IUserService userService)
        {
            this.feedbackService = feedbackService; 
            this.userService = userService;
        }

        [HttpGet("accept-feedback")]
        public async Task<IActionResult> GetAcceptFeedbackListByFixerId()
        {
            try
            {
                // Get id of current log in user 
                int userId = userService.GetCurrentLoginUserId(Request.Headers["Authorization"]);

                var s = await feedbackService.GetFeedbacksByFixerIdAndStatusIsAccept(userId);

                return Ok(s);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

        [HttpGet("close-feedback")]
        public async Task<IActionResult> GetCLoseFeedbackListByFixerId()
        {
            try
            {
                // Get id of current log in user 
                int userId = userService.GetCurrentLoginUserId(Request.Headers["Authorization"]);

                var s = await feedbackService.GetFeedbacksByFixerIdAndStatusIsClose(userId);

                return Ok(s);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

        [HttpGet("feedback/{feedbackId:int}")]
        public async Task<IActionResult> GetfeedbackById(int feedbackId)
        {
            try
            {
                return Ok(await feedbackService.GetFeedbackById(feedbackId));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

        [HttpPut("feedback/{feedbackId:int}")]
        public async Task<IActionResult> ProcessFeedback(int feedbackId, string message)
        {
            try
            {
                // Get id of current log in user 
                int fixerId = userService.GetCurrentLoginUserId(Request.Headers["Authorization"]);

                await feedbackService.ProcessFeedback(fixerId, feedbackId, message);
                return Ok("Process completed successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }
    }
}
