using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Models;
using DataAccess.IServices;

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
        public async Task<IEnumerable<Feedback>> GetFeedbacks()
        {
            return await _service.GetAllFeedback();
        }

        // GET: api/Feedback/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Feedback>> GetFeedback(int id)
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
        public async Task<IActionResult> PutFeedback(int id, Feedback feedback)
        {
            try
            {
                await _service.CreateFeedback(feedback);
            }
            catch (DbUpdateConcurrencyException)
            {
                
                    throw;
            }

            return NoContent();
        }

        // POST: api/Feedback
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<Feedback>> PostFeedback(Feedback feedback)
        //{
        //    _service.Feedbacks.Add(feedback);
        //    try
        //    {
        //        await _service.SaveChangesAsync();
        //    }
        //    catch (DbUpdateException)
        //    {
        //        if (FeedbackExists(feedback.Id))
        //        {
        //            return Conflict();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return CreatedAtAction("GetFeedback", new { id = feedback.Id }, feedback);
        //}

        // DELETE: api/Feedback/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteFeedback(int id)
        //{
        //    var feedback = await _service.Feedbacks.FindAsync(id);
        //    if (feedback == null)
        //    {
        //        return NotFound();
        //    }

        //    _service.Feedbacks.Remove(feedback);
        //    await _service.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool FeedbackExists(int id)
        //{
        //    return _service.Feedbacks.Any(e => e.Id == id);
        //}
    }
}
