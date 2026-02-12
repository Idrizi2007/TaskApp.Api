namespace TaskApp.Domain.Entities
{
    public class TaskItem
    {
        public int Id { get; private set; }
        public string Title { get; private set; } = "";
        public string?  Description { get; private  set; }
        public bool IsCompleted { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public TaskItem(string title,string? description )
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title is required");
            Title = title;
            Description = description;
            CreatedAt = DateTime.UtcNow;
            IsCompleted = false;
            
        }

        private TaskItem()
        {

        }

        public void Complete()
        {
            if (IsCompleted)
           
                throw new InvalidOperationException("The task is already completed");
               
            IsCompleted = true;
        }

        public void IsnotComplete(string title, string? description)
        {
            if (IsCompleted)
                throw new InvalidOperationException("The task is already completed");
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title is empty");

            Title = title;
            Description = description;
        }

        public void EnsureCanBeDeleted()
        {
            if (IsCompleted)
            {
                throw new InvalidOperationException("The task is already completed");
            }
        }

      
    }
}
