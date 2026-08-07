using TraineeManagement.API.DTOs;

namespace TraineeManagement.API.Services;

public interface IUserService
{
    Task<UserResponse?> CreateAsync(CreateUserRequest request);
}