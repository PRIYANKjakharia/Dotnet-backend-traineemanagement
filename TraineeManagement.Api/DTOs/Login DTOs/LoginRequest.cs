using System.ComponentModel.DataAnnotations;
namespace TraineeManagement.API.DTOs;
public class LoginRequest
{
    [Required (ErrorMessage = "Userame is required")]
    [MaxLength(50 , ErrorMessage = "Userame must be below 50 characters")]
    public string Username {get; set;} = string.Empty;
    [Required (ErrorMessage = "Password is required")]
    public string Password {get; set;} = string.Empty;
}