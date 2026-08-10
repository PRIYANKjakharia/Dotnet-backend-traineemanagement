using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TraineeManagement.API.DTOs;
using TraineeManagement.API.Services;

namespace TraineeManagement.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/learningtasks")]
    public class LearningTaskController : ControllerBase
    {
        private readonly ILearningTaskService _service;
        public LearningTaskController(ILearningTaskService service)
        {
            _service = service;
        }


        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateLearningTaskRequest request)
        {
            var result = await _service.CreateAsync(request);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var Mentor = await _service.GetByIdAsync(id);
            if (Mentor == null) return NotFound( new{ message = "id not found"} );
            return Ok(Mentor);
        }


        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var success = await _service.DeleteAsync(id);
            if (!success) return NotFound( new{ message = "Id Not Found"} );
            return Ok( new{ message = "deleted sucessfully"} );
        }


        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id , UpdateLearningTaskRequest request)
        {
            string result = await _service.UpdateAsync(id, request);
            if(result == "Id Not Found")
            {
                return NotFound( new { message = result});
            } 
            return Ok( new{ message = result} );
        }
    }
}