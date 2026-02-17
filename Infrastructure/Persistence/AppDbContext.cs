using Microsoft.EntityFrameworkCore;
using TaskApp.Api.Domain.Entities;

namespace TaskApp.Api.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public DbSet<TaskItem> Tasks { get; set; }
        public DbSet<User> Users => Set<User>();

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .OwnsOne(u => u.RefreshToken);
        }
    }
}
