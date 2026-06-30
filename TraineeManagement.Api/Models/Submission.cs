using System.ComponentModel.DataAnnotations;

namespace TraineeManagement.API.Models;

public class Submission
{
    [Key]
    public int Id { get; set; }
    public int? TaskAssignmentId { get; set; }
    public string? SubmissionUrl { get; set; }
    public string? Notes { get; set; }
    public string? Remarks { get; set; }
    public DateTime? SubmissionDate { get; set; }
    public TaskAssignment? TaskAssignment { get; set; }
    public string? Status { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public ICollection<SubmissionFile>? Files { get; set; } = new List<SubmissionFile>();
}
