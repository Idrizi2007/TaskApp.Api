using TaskApp.Api.DTOS;
using TaskApp.Domain.Entities;

namespace TaskApp.Api.Services
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskItem>> GetAllAsync();
        Task<TaskItem> GetByIdAsync(int id);
        Task<TaskDto> CreateAsync(CreateTaskDto dto);
        Task CompleteAsync(int id);
        Task<IEnumerable<TaskItem>> GetAllAsync(int page, int pageSize);


    }
}
