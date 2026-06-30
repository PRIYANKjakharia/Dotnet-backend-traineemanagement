using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TraineeManagement.API.DTOs;
using TraineeManagement.API.Services;

namespace TraineeManagement.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/taskAssignments")]
    public class TaskAssignmentController : ControllerBase
    {
        private readonly ITaskAssignmentService _service;
        public TaskAssignmentController(ITaskAssignmentService service)
        {
            _service = service;
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateTaskAssignmentRequest T)
        {
            var result = await _service.CreateAsync(T);
            if(result == null)
            {
                return BadRequest(new{ message = "trainee/mentor/LearningTask Id not found  Or AssignedDate is greater than DueDate" });
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
            var taskAssignment = await _service.GetByIdAsync(id);
            if (taskAssignment == null) return NotFound( new{ message = "id not found"} );
            return Ok(taskAssignment);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var success = await _service.DeleteAsync(id);
            if (!success) return NotFound( new{ message = "Id Not Found"} );
            return Ok( new{ message = "deleted sucessfully"} );
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id , UpdateTaskAssignmentRequest request)
        {
            string result = await _service.UpdateAsync(id, request);
            if(result != "Updated SucessFully")
            {
                return NotFound( new { meessage = result});
            } 
            return Ok( new{ message = result} );
        }
    }
}