using TraineeManagement.API.DTOs;

namespace TraineeManagement.API.Services;

public interface IFileStorageService
{
    Task<string> SaveFileAsync(IFormFile file);
    Task<Stream> OpenReadAsync(string StoredFileName);
    Task<bool> ExistsAsync(string StoredFileName);
    Task DeleteAsync(string StoredFileName);
}