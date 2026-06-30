using TraineeManagement.API.DTOs;
using TraineeManagement.API.Models;

namespace TraineeManagement.API.Services;

public interface ISubmissionFileService
{
    Task<UploadFileResponse?> UploadAsync(int submissionId , IFormFile file);
    Task<(Stream stream , string contentType , string fileName)?> DownloadAsync(int fileId);
    Task<bool> DeleteAsync(int fileId);
}







