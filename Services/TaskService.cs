using Microsoft.EntityFrameworkCore;
using TaskApp.Api.DTOS;
using TaskApp.Api.Infrastructure.Persistence;
using TaskApp.Domain.Entities;
using TaskApp.Api.Exceptions;

namespace TaskApp.Api.Services
{
    public class TaskService : ITaskService
    {
        private readonly AppDbContext _context;

        public TaskService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TaskItem>> GetAllAsync()
        {
            return await _context.Tasks.ToListAsync();
        }

        public async Task<TaskItem> GetByIdAsync(int id)
        {
            var task = await _context.Tasks.FindAsync(id);

            if (task is null)
                throw new NotFoundException($"Task with id '{id}' was not found.");

            return task;
        }

        public async Task<TaskDto> CreateAsync(CreateTaskDto dto)
        {
            var task = new TaskItem(dto.Title, dto.Description);

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                IsCompleted = task.IsCompleted,
                CreatedAt = task.CreatedAt
            };
        }

        public async Task CompleteAsync(int id)
        {
            var task = await _context.Tasks.FindAsync(id);

            if (task is null)
                throw new NotFoundException($"Task with id '{id}' was not found.");

            task.Complete();

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TaskItem>> GetAllAsync(int page, int pageSize)
        {
            page = page < 1 ? 1 : page;
            pageSize = pageSize is < 1 or > 50 ? 10 : pageSize;

            return await _context.Tasks
                .OrderByDescending(t => t.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

    }
}
