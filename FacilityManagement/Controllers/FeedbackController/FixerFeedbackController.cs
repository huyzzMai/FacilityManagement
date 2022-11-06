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

        [HttpGet("user/{id:int}")]
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

        [HttpGet("feedback/{id:int}")]
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

        [HttpPut]
        public async Task<IActionResult> ProcessFeedback()
        {
            try
            {
                return null;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }
    }
}
