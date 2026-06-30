using System.ComponentModel.DataAnnotations;

namespace TraineeManagement.API.DTOs;

public class CreateTaskAssignmentRequest
{
    public int Id { get; set; }

    [Required(ErrorMessage = "TraineeId is required")]
    public int? TraineeId { get; set; }

    [Required(ErrorMessage = "MentorId is required")]
    public int? MentorId { get; set; }

    [Required(ErrorMessage = "LearningTaskId is required")]
    public int? LearningTaskId { get; set; }

    // [Required(ErrorMessage = "AssignedDate is required")]
    // public DateTime? AssignedDate { get; set; }

    // [Required(ErrorMessage = "DueDate is required")]
    // public DateTime? DueDate { get; set; }

    [Required(ErrorMessage = "Status is required")]
    public string? Status { get; set; }
    
    public string Remarks { get; set; }
}