using TraineeManagement.API.DTOs;
using TraineeManagement.API.Models;

namespace TraineeManagement.API.Services;

public interface IReviewService
{
    Task<List<ReviewResponse>> GetAllAsync();
    Task<ReviewResponse?> GetByIdAsync(int id);
    Task<ReviewResponse> CreateAsync(CreateReviewRequest request);
}







