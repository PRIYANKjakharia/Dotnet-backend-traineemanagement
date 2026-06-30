using System.ComponentModel.DataAnnotations;

namespace TraineeManagement.API.DTOs;

public class SubmissionResponse
{
    public int? Id {get; set;}
    public int? TaskAssignmentId { get; set; }

    public string? SubmissionUrl { get; set; }
    
    public string? Notes { get; set; }

    public DateTime? SubmissionDate { get; set; }

    public string? Status { get; set; }

    public string? TaskTitle {get; set;}
    public string? TraineeName {get; set;}
}