namespace TodoList;

public class TodoItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public bool IsDone { get; set; }
    public DateTime? DueDate { get; set; }
    public List<string> Tags { get; set; } = new();
    public DateTimeOffset? CompletedAt { get; set; }
}
