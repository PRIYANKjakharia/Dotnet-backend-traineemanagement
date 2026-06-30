using System.ComponentModel.DataAnnotations;

namespace TraineeManagement.API.Models;

public class Review
{
    [Key]
    public int Id { get; set; }
    public int? SubmissionId { get; set; }
    public int? MentorId { get; set; }
    public int? Score { get; set; }
    public string? Feedback { get; set; }
    public string? ReviewStatus { get; set; }
    public DateTime? SubmissionDate { get; set; }
    public TaskAssignment? TaskAssignment { get; set; }
    public DateTime? ReviewedDate { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public Submission? Submission { get; set; }
    public Mentor? Mentor { get; set; }
}
