using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TraineeManagement.API.Models;

public class TaskAssignment
{
    [Key]
    public int Id { get; set; }
    public int? TraineeId { get; set; }
    public string? TraineeName { get; set; }
    public int? MentorId { get; set; }
    public string? MentorName { get; set; }
    public int? LearningTaskId { get; set; }
    public string? LearningTaskTitle { get; set; }
    public DateTime? AssignedDate { get; set; }
    public DateTime? DueDate { get; set; }
    public string? Remarks { get; set; }
    public string? Status { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public Trainee? Trainee { get; set; }
    public Mentor? Mentor { get; set; }
    public LearningTask? LearningTask { get; set; }
}
