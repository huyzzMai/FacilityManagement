using DataAccess.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FacilityManagement.Controllers.FeedbackController
{
    [Route("api/fixerfeedback")]
    [ApiController]
    //[Authorize(Roles = "Fixer")]
    public class FixerFeedbackController : ControllerBase
    {
        private readonly IFeedbackService feedbackService;

        public FixerFeedbackController(IFeedbackService feedbackService)
        {
            this.feedbackService = feedbackService; 
        }

        [HttpGet("user/{userId:int}")]
        public async Task<IActionResult> GetFeedbackListByFixerId(int userId)
        {
            try
            {
                return Ok(await feedbackService.GetAllFeedbackByUserId(userId));
            }
            catch(Exception ex)
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

        [HttpPut("feedback/{feedbackId:int}/fixer/{fixerId:int}")]
        public async Task<IActionResult> ProcessFeedback(int fixerId, int feedbackId, string message)
        {
            try
            {
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
