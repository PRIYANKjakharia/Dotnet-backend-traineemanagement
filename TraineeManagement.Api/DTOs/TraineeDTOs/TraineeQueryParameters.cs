using System.ComponentModel.DataAnnotations;

namespace TraineeManagement.API.DTOs;

public class TraineeQueryParameters{
    public int PageNumber{ get; set; } = 1;
    public int PageSize{ get; set; } = 10;
    public string? Search { get; set; }

    // [AllowedValues("Active", "Inactive",ErrorMessage ="Status must be fromgggg Active, Inactive")]
    public string? Status { get; set; }
 
}
 