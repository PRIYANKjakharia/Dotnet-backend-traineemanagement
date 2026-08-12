using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TraineeManagement.API.DTOs;
using TraineeManagement.API.Services;

namespace TraineeManagement.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/submissions")]
    public class SubmissionController : ControllerBase
    {
        private readonly ISubmissionService _service;
        public SubmissionController(ISubmissionService service)
        {
            _service = service;
        }


        [Authorize(Roles = "trainee")]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateSubmissionRequest request)
        {
            var result = await _service.CreateAsync(request);
            if(result == null)
            {
                return BadRequest(new{ message = "taskId not found" });
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
            var submission = await _service.GetByIdAsync(id);
            if (submission == null) return NotFound( new{ message = "id not found"} );
            return Ok(submission);
        }

    }
}