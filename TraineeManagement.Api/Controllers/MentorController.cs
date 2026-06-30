using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TraineeManagement.API.DTOs;
using TraineeManagement.API.Services;

namespace TraineeManagement.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/mentors")]
    public class MentorController : ControllerBase
    {
        private readonly IMentorService _service;
        public MentorController(IMentorService service)
        {
            _service = service;
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateMentorRequest T)
        {
            var result = await _service.CreateAsync(T);
            if(result == null)
            {
                return BadRequest(new{ message = "Email already Exists" });
            }
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
            return Ok( new{ message = "Deleted Sucessfully"} );
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id , UpdateMentorRequest request)
        {
            string result = await _service.UpdateAsync(id, request);
            if(result == "Id Not Found")
            {
                return NotFound( new { meessage = result});
            } else if(result == "Email already exists")
            {
                return BadRequest( new { meessage = result});
            }
            return Ok( new{ message = result} );
        }
    }
}