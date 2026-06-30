using System.ComponentModel.DataAnnotations;

namespace TraineeManagement.API.DTOs;

public class CreateReviewRequest
{

    [Required(ErrorMessage = "SubmissionId is required")]
    public int? SubmissionId { get; set; }

    [Required(ErrorMessage = "MentorId is required")]
    public int? MentorId { get; set; }

    [Required(ErrorMessage = "Feedback is required")]
    public string? Feedback { get; set; }
    [Required(ErrorMessage = "ReviewStatus is required")]
    public string? ReviewStatus { get; set; }
    public int? Score { get; set; }

    // [Required(ErrorMessage = "AssignedDate is required")]
    // public DateTime? AssignedDate { get; set; }

    // [Required(ErrorMessage = "DueDate is required")]
    // public DateTime? DueDate { get; set; }

    [Required(ErrorMessage = "ReviewedDate is required")]
    public DateTime? ReviewedDate { get; set; }
}