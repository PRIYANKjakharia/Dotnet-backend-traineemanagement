using TraineeManagement.API.DTOs;
using TraineeManagement.API.Models;

namespace TraineeManagement.API.Services;

public interface ISubmissionService
{
    Task<List<SubmissionResponse>> GetAllAsync();
    Task<SubmissionResponse?> GetByIdAsync(int id);
    Task<SubmissionResponse> CreateAsync(CreateSubmissionRequest request);
}







