using Microsoft.AspNetCore.Mvc;
using TraineeManagement.API.DTOs;
using TraineeManagement.API.Services;

namespace TraineeManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var response =  await _authService.LoginAsync(request);
        if(response == null)
        {
            return Unauthorized( new { Message = "Invalid username or password"});
        }
        return Ok(response);
    }
}