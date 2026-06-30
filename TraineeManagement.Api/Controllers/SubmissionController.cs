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
        private readonly IFileStorageService _fileStorageService;
        public SubmissionController(ISubmissionService service , IFileStorageService fileStorageService)
        {
            _service = service;
            _fileStorageService = fileStorageService;
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateSubmissionRequest T)
        {
            var result = await _service.CreateAsync(T);
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



        // [HttpPost("testupload")]
        // public async Task<IActionResult> TestUpload(IFormFile file)
        // {
        //     string fileName = await _fileStorageService.SaveFileAsync(file);
        //     bool exists = await _fileStorageService.ExistsAsync(fileName);
        //     return Ok(new{
        //         StoredName = fileName,
        //         Exists = exists
        //     });
        // }
    }
}