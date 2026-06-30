using TraineeManagement.API.DTOs;

namespace TraineeManagement.API.Services;

public interface ILearningTaskService
{
    Task<LearningTaskResponse?> GetByIdAsync(int id);
    Task<LearningTaskResponse> CreateAsync(CreateLearningTaskRequest request);
    Task<string> UpdateAsync(int id , UpdateLearningTaskRequest request);
    Task<bool> DeleteAsync(int id);
    // Task<List<LearningTaskResponse>> Search(String s);
    Task<List<LearningTaskResponse>> GetAllAsync();
}







