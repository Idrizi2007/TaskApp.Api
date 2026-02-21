using TaskApp.Api.DTOS;
using TaskApp.Api.Domain.Entities;

public interface ITaskService
{
    Task<IEnumerable<TaskItem>> GetAllAsync(Guid userId, bool isAdmin, int page, int pageSize);
    Task<TaskItem> GetByIdAsync(int id, Guid userId, bool isAdmin);
    Task<TaskDto> CreateAsync(CreateTaskDto dto, Guid userId);
    Task CompleteAsync(int id, Guid userId, bool isAdmin);
}