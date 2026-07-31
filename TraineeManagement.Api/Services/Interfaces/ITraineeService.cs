using TraineeManagement.API.DTOs;

namespace TraineeManagement.API.Services;

public interface ITraineeService
{
    Task<TraineeResponse?> GetById(int id);
    Task<TraineeResponse> Create(CreateTraineeRequest request);
    Task<string> Update(int id , UpdateTraineeRequest request);
    Task<bool> Delete(int id);
    Task<PagedResponse<TraineeResponse>> GetAllAsync( TraineeQueryParameters query);
}







