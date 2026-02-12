using Microsoft.EntityFrameworkCore;
using TaskApp.Api.DTOS;
using TaskApp.Api.Infrastructure.Persistence;
using TaskApp.Domain.Entities;

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

        // CREATE — uses DTO, creates domain entity INSIDE service
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

        public async Task<bool> CompleteAsync(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
                return false;

            task.Complete();

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
