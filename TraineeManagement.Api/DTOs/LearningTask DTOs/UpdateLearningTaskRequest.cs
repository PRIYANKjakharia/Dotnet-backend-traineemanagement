using System.ComponentModel.DataAnnotations;
using Microsoft.VisualBasic;

namespace TraineeManagement.API.DTOs;

public class UpdateLearningTaskRequest
{
    [Required (ErrorMessage = "Title is required")]
    [MaxLength(100 , ErrorMessage = "Title must be below 100 characters")]
    public string? Title { get; set; }

    [Required(ErrorMessage = "Description is required")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "ExpectedTechStack is required")]
    public string? ExpectedTechStack { get; set; }

    [Required(ErrorMessage = "DueDate is required")]
    public DateTime DueDate { get; set; }

    [Required(ErrorMessage = "Status is required")]
    [AllowedValues("Active", "Inactive",ErrorMessage ="Status must be from Active, Inactive")]
    public string? Status { get; set; }
}