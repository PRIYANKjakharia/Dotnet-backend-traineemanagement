using Microsoft.EntityFrameworkCore;
using TraineeManagement.API.Data;
using TraineeManagement.API.DTOs;
using TraineeManagement.API.Interfaces;
 
namespace TraineeManagement.API.Services;
 
public class ProcessingJobService : IProcessingJobService
{
    private readonly AppDbContext _context;
 
    public ProcessingJobService(AppDbContext context)
    {
        _context = context;
    }
 
    public async Task<ProcessingJobResponse?> GetByIdAsync(int id)
    {
        var job = await _context.ProcessingJobs
            .FirstOrDefaultAsync(_job => _job.Id == id);
 
        if (job == null)
            return null;
 
        return new ProcessingJobResponse
        {
            Id = job.Id,
            MessageId = job.MessageId,
            CorrelationId = job.CorrelationId,
            SubmissionId = job.SubmissionId,
            FileId = job.FileId,
            Status = job.Status,
            Attempts = job.Attempts,
            ErrorSummary = job.ErrorSummary,
            StartedAt = job.StartedAt,
            CompletedAt = job.CompletedAt,
            CreatedAt = job.CreatedAt,
            UpdatedAt = job.UpdatedAt
        };
    }
}
 