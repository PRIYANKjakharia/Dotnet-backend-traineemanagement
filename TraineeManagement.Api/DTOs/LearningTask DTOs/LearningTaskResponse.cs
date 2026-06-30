using System.ComponentModel.DataAnnotations;
using Microsoft.VisualBasic;

namespace TraineeManagement.API.DTOs;

public class LearningTaskResponse
{
    public int Id { get; set; }
    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? ExpectedTechStack { get; set; }

    public DateTime DueDate { get; set; }
    public string? Status { get; set; }
}