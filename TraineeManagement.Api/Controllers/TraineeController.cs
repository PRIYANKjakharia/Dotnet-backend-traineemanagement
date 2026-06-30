using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using TraineeManagement.API.DTOs;
using TraineeManagement.API.Services;

namespace TraineeManagement.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/trainees")]
    public class TraineeController : ControllerBase
    {
        private readonly ITraineeService _service;
        public TraineeController(ITraineeService service)
        {
            _service = service;
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateTraineeRequest T)
        {
            var result = await _service.Create(T);
            if(result == null)
            {
                return BadRequest(new{ message = "Email already Exists" });
            }
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll( [FromQuery]TraineeQueryParameters query)
        {
            var result = await _service.GetAllAsync(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var trainee = await _service.GetById(id);
            if (trainee == null) return NotFound( new{ message = "id not found"} );
            return Ok(trainee);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.Delete(id);
            if (!success) return NotFound( new{ message = "Id Not Found"} );
            return Ok(new{ message = "Deleted sucessfully"});
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id , UpdateTraineeRequest request)
        {
            string result = await _service.Update(id, request);
            if(result == "Id Not Found")
            {
                return NotFound( new { meessage = result});
            } else if(result == "Email already exists")
            {
                return BadRequest( new { meessage = result});
            }
            return Ok( new{ message = result} );
        }

        [HttpGet("test")]
        public IActionResult test ()
        {
            throw new Exception("testing exception");
        }

    }
}