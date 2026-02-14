using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TaskApp.Api.Domain.Entities;




namespace TaskApp.Api.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public DbSet <TaskItem> Tasks { get; set; }
        public DbSet<User> Users => Set<User>();

        public AppDbContext(DbContextOptions<AppDbContext> options)
                : base(options)
        {
            
        }
    }
}
