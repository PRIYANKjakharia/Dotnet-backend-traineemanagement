using System.ComponentModel.DataAnnotations;

namespace TraineeManagement.API.DTOs;

public class CreateSubmissionRequest
{
    [Required(ErrorMessage = "TaskAssignmentId is required")]
    public int? TaskAssignmentId { get; set; }

    [Required(ErrorMessage = "SubmissionUrl is required")]
    public string? SubmissionUrl { get; set; }
    
    public string? Notes { get; set; }

    [Required(ErrorMessage = "SubmissionDate is required")]
    public DateTime? SubmissionDate { get; set; }

    [Required(ErrorMessage = "Status is required")]
    public string? Status { get; set; }
}