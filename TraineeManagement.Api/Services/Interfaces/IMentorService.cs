using TraineeManagement.API.DTOs;

namespace TraineeManagement.API.Services;

public interface IMentorService
{
    Task<MentorResponse?> GetByIdAsync(int id);
    Task<MentorResponse> CreateAsync(CreateMentorRequest request);
    Task<string> UpdateAsync(int id , UpdateMentorRequest request);
    Task<bool> DeleteAsync(int id);
    // Task<List<MentorResponse>> Search(String s);
    Task<List<MentorResponse>> GetAllAsync();
}







