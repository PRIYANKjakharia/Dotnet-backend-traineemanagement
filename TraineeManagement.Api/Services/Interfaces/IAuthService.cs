using TraineeManagement.API.DTOs;

namespace TraineeManagement.API.Services;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest request);
}