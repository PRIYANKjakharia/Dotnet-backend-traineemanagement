namespace TraineeManagement.API.Services;

using TraineeManagement.API.DTOs;

public interface ITrainingDirectoryClient
{
    Task<DirectoryProfileDto?> GetProfileAsync(int traineeId, CancellationToken cancellationToken);
}