using TraineeManagement.API.DTOs;
 
namespace TraineeManagement.API.Interfaces;
 
public interface IProcessingJobService
{
    Task<ProcessingJobResponse?> GetByIdAsync(int id);
}
 