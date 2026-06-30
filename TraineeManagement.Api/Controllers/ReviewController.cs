using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TraineeManagement.API.DTOs;
using TraineeManagement.API.Services;

namespace TraineeManagement.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/reviews")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _service;
        public ReviewController(IReviewService service)
        {
            _service = service;
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateReviewRequest T)
        {
            var result = await _service.CreateAsync(T);
            if(result == null)
            {
                return BadRequest(new{ message = "Submission or Mentor Id not found" });
            }
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var review = await _service.GetByIdAsync(id);
            if (review == null) return NotFound( new{ message = "id not found"} );
            return Ok(review);
        }
    }
}