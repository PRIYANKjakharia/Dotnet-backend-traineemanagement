using System.ComponentModel.DataAnnotations;

namespace TraineeManagement.API.DTOs;

public class UpdateSubmissionRequest
{
    [Required(ErrorMessage = "TaskAssignmentId is required")]
    public int? TaskAssignmentId { get; set; }

    [Required(ErrorMessage = "SubmissionUrl is required")]
    public string? SubmissionUrl { get; set; }
    
    public string? Notes { get; set; }

    [Required(ErrorMessage = "SubmissionDate is required")]
    public DateTime? SubmissionDate { get; set; }

    [Required(ErrorMessage = "Status is required")]
    [AllowedValues("Submitted", "Pending",ErrorMessage ="Status must be from Assigned, Submitted or Pending")]
    public string? Status { get; set; }
}