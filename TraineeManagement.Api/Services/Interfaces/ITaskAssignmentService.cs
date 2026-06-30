using TraineeManagement.API.DTOs;
using TraineeManagement.API.Models;

namespace TraineeManagement.API.Services;

public interface ITaskAssignmentService
{
    Task<List<TaskAssignmentResponse>> GetAllAsync();
    Task<TaskAssignmentResponse?> GetByIdAsync(int id);
    Task<TaskAssignmentResponse?> CreateAsync(CreateTaskAssignmentRequest request);
    Task<string> UpdateAsync(int id , UpdateTaskAssignmentRequest request);
    Task<bool> DeleteAsync(int id);
}







