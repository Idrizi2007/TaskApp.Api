using Microsoft.EntityFrameworkCore;
using TaskApp.Api.Domain.Entities;
using TaskApp.Api.DTOS;
using TaskApp.Api.Exceptions;
using TaskApp.Api.Infrastructure.Persistence;

namespace TaskApp.Api.Services;

public class TaskService : ITaskService
{
    private readonly AppDbContext _context;

    public TaskService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TaskItem>> GetAllAsync(Guid userId, bool isAdmin, int page, int pageSize)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;

        var query = _context.Tasks.AsQueryable();

        if (!isAdmin)
            query = query.Where(t => t.UserId == userId);

        return await query
            .OrderByDescending(t => t.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<TaskItem> GetByIdAsync(int id, Guid userId, bool isAdmin)
    {
        var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);

        if (task is null)
            throw new NotFoundException("Task not found.");

        if (!isAdmin && task.UserId != userId)
            throw new UnauthorizedAccessException("Forbidden.");

        return task;
    }

    public async Task<TaskDto> CreateAsync(CreateTaskDto dto, Guid userId)
    {
        var task = new TaskItem(dto.Title, dto.Description, userId);

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        // If your TaskDto differs, adjust mapping here
        return new TaskDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            IsCompleted = task.IsCompleted,
            CreatedAt = task.CreatedAt
        };
    }

    public async Task CompleteAsync(int id, Guid userId, bool isAdmin)
    {
        var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);

        if (task is null)
            throw new NotFoundException("Task not found.");

        if (!isAdmin && task.UserId != userId)
            throw new UnauthorizedAccessException("Forbidden.");

        task.Complete();
        await _context.SaveChangesAsync();
    }
}