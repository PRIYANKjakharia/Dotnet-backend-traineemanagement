using System.ComponentModel.DataAnnotations;

namespace TraineeManagement.API.DTOs;

public class UpdateTaskAssignmentRequest
{
    [Required(ErrorMessage = "Id is required")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Status is required")]
    public string? Status { get; set; }
}