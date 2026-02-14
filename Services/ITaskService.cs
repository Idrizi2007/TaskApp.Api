using TaskApp.Api.DTOS;
namespace TaskApp.Api.Domain.Entities;


public interface ITaskService
{
    Task<IEnumerable<TaskItem>> GetAllAsync(Guid userId, int page, int pageSize);
    Task<TaskItem> GetByIdAsync(int id, Guid userId);
    Task<TaskDto> CreateAsync(CreateTaskDto dto, Guid userId);
    Task CompleteAsync(int id, Guid userId);
}
