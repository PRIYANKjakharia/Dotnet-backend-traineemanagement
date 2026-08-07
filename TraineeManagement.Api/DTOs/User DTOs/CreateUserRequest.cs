using System.ComponentModel.DataAnnotations;

namespace TraineeManagement.API.DTOs;

public class CreateUserRequest
{
    [Required(ErrorMessage = "Username is required")]
    [MaxLength(50, ErrorMessage = "Username must be below 50 characters")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Valid Email is required")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Role is required")]
    [AllowedValues("mentor", "trainee", ErrorMessage = "Role must be mentor or trainee")]
    public string Role { get; set; } = string.Empty;

    // Trainee/Mentor information
    [Required(ErrorMessage = "FirstName is required")]
    [MaxLength(50)]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "LastName is required")]
    [MaxLength(50)]
    public string LastName { get; set; } = string.Empty;

    // Only required for trainee
    public string? TechStack { get; set; }

    // Only required for mentor
    public string? Expertise { get; set; }

    [Required(ErrorMessage = "Status is required")]
    [AllowedValues("Active", "Inactive")]
    public string Status { get; set; } = string.Empty;
}