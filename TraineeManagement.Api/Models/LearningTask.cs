using System.ComponentModel.DataAnnotations;

namespace TraineeManagement.API.Models;

public class LearningTask
{
    [Key]
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }
    public string? ExpectedTechStack { get; set; }
    public DateTime DueDate { get; set; }
    public string? Status { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}
