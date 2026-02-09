using TaskApp.Api.DTOS;

namespace TaskApp.Api.Services
{
    public class TaskService : ITaskService
    {
        private readonly List<TaskDto> _tasks = new()
        {
            new TaskDto
            {
                Id = 1,
                Title = "First Task",
                IsCompleted = false,
                DueDate = DateTime.UtcNow.AddDays(1)
            },
            new TaskDto
            {
                Id = 2,
                Title = "Second Task",
                IsCompleted = true,
                DueDate = DateTime.UtcNow.AddDays(-1)
            },
            
        };

        public IEnumerable<TaskDto> GetAll()
        {
            return _tasks;
        }
        

    }

}

