namespace TraineeManagement.API.DTOs;

public class DirectoryProfileDto
{
    public int TraineeId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string TierCode { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public string Track { get; set; } = string.Empty;
}