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
        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateLearningTaskRequest T)
        {
            var result = await _service.CreateAsync(T);
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
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var success = await _service.DeleteAsync(id);
            if (!success) return NotFound( new{ message = "Id Not Found"} );
            return Ok( new{ message = "deleted sucessfully"} );
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id , UpdateLearningTaskRequest request)
        {
            string result = await _service.UpdateAsync(id, request);
            if(result == "Id Not Found")
            {
                return NotFound( new { meessage = result});
            } 
            return Ok( new{ message = result} );
        }
    }
}