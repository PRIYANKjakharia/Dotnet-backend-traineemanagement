using System.ComponentModel.DataAnnotations;

namespace TraineeManagement.API.DTOs;

public class UpdateMentorRequest
{
    [Required (ErrorMessage = "FirstName is required")]
    [MaxLength(50 , ErrorMessage = "LastName must be below 50 characters")]
    public string? FirstName { get; set; }
    [Required(ErrorMessage = "LastName is required")]
    [MaxLength(50 , ErrorMessage = "LastName must be below 50 characters")]
    public string? LastName { get; set; }
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Valid Email is required")]
    public string? Email { get; set; }
    [Required(ErrorMessage = "Expertise is required")]
    public string? Expertise { get; set; }
    [Required(ErrorMessage = "Status is required")]
    [AllowedValues("Active", "Inactive",ErrorMessage ="Status must be from Active, Inactive")]
    public string? Status { get; set; }
}