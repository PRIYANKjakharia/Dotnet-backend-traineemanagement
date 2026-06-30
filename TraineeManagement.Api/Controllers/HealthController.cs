using Microsoft.AspNetCore.Mvc;

namespace TraineeManagement.Api.Controllers
{
    [ApiController]
    [Route("api/health")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new
            {
                status ="Running",
                application = "Trainee Management API",
                timestamp = DateTime.Now
            });
        }
    }
}
