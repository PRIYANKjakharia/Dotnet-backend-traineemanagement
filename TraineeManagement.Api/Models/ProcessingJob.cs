namespace TraineeManagement.API.Models;
 
public class ProcessingJob
{
    public int Id { get; set; }
 
    public Guid MessageId { get; set; }
 
    public Guid CorrelationId { get; set; }
 
    public int SubmissionId { get; set; }
 
    public int FileId { get; set; }
 
    public string Status { get; set; } = "Queued";
 
    public int Attempts { get; set; }
 
    public string? ErrorSummary { get; set; }
 
    public DateTime? StartedAt { get; set; }
 
    public DateTime? CompletedAt { get; set; }
 
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
 
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}