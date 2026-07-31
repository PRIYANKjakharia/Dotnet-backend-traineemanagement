using System.ComponentModel.DataAnnotations;

namespace TraineeManagement.API.DTOs;

public class UpdateReviewRequest
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

    [Required(ErrorMessage = "ReviewedDate is required")]
    public DateTime? ReviewedDate { get; set; }
}