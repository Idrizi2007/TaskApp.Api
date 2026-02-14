public class TaskItem
{
    public int Id { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public bool IsCompleted { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public Guid UserId { get; private set; }

    private TaskItem() { }

    public TaskItem(string title, string description, Guid userId)
    {
        Title = title;
        Description = description;
        UserId = userId;
        CreatedAt = DateTime.UtcNow;
        IsCompleted = false;
    }

    public void Complete()
    {
        IsCompleted = true;
    }
}
