using TaskApp.Api.DTOS;
using TaskApp.Domain.Entities;

namespace TaskApp.Api.Services
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskItem>> GetAllAsync();
        Task<TaskItem> CreateAsync(TaskItem task);
    }

}

