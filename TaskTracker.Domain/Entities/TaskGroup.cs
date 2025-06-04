namespace TaskTracker.Domain.Entities;

public class TaskGroup
{
   public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public int UserId { get; set; }
    public User? User { get; set; }

    public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
}
