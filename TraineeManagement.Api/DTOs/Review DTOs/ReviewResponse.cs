using System.ComponentModel.DataAnnotations;

namespace TraineeManagement.API.DTOs;

public class ReviewResponse
{

    public int Id  { get; set; }
    public int? SubmissionId { get; set; }
    public int? MentorId { get; set; }
    public string? Feedback { get; set; }
    public string? ReviewStatus { get; set; }
    public DateTime? ReviewedDate { get; set; }
    public int? Score { get; set; }
    public string? MentorName { get; set; }
    public string? SubmissionUrl { get; set; }

    
}