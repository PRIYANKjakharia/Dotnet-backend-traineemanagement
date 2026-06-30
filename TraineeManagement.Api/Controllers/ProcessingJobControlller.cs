using Microsoft.AspNetCore.Mvc;
using TraineeManagement.API.Interfaces;
 
namespace TraineeManagement.API.Controllers;
 
[ApiController]
[Route("api/processing-jobs")]
public class ProcessingJobController : ControllerBase
{
    private readonly IProcessingJobService _service;
 
    public ProcessingJobController(IProcessingJobService service)
    {
        _service = service;
    }
 
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var job = await _service.GetByIdAsync(id);
 
        if (job == null)
            return NotFound();
 
        return Ok(job);
    }
}