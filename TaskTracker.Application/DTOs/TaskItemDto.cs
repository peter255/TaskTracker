namespace TaskTracker.Application.DTOs;

public class TaskItemDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Status { get; set; } = "Pending";
    public DateTime DueDate { get; set; }
    public int TaskGroupId { get; set; }
}
