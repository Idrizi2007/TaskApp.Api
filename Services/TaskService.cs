using Microsoft.EntityFrameworkCore;
using TaskApp.Api.DTOS;
using TaskApp.Api.Exceptions;
using TaskApp.Api.Infrastructure.Persistence;
using TaskApp.Api.Domain.Entities;




namespace TaskApp.Api.Services
{
    public class TaskService : ITaskService
    {
        private readonly AppDbContext _context;

        public TaskService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TaskItem>> GetAllAsync(
            Guid userId,
            int page,
            int pageSize)
        {
            page = page < 1 ? 1 : page;
            pageSize = pageSize is < 1 or > 50 ? 10 : pageSize;

            return await _context.Tasks
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<TaskItem> GetByIdAsync(int id, Guid userId)
        {
            return await _context.Tasks
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId)
                ?? throw new NotFoundException($"Task with id '{id}' was not found.");
        }

        public async Task<TaskDto> CreateAsync(CreateTaskDto dto, Guid userId)
        {
            var task = new TaskItem(dto.Title, dto.Description, userId);

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

        public async Task CompleteAsync(int id, Guid userId)
        {
            var task = await _context.Tasks
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId)
                ?? throw new NotFoundException($"Task with id '{id}' was not found.");

            task.Complete();
            await _context.SaveChangesAsync();
        }
    }
}
