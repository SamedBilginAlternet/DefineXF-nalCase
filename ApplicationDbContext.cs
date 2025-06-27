using Microsoft.EntityFrameworkCore;
using DefineXFinalCase.Domain.Entities;

namespace DefineXFinalCase.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Project> Projects => Set<Project>();
        public DbSet<DefineXFinalCase.Domain.Entities.Task> Tasks => Set<DefineXFinalCase.Domain.Entities.Task>();
        public DbSet<TaskComment> TaskComments => Set<TaskComment>();
        public DbSet<TaskAttachment> TaskAttachments => Set<TaskAttachment>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Ýliþkiler ve soft delete filtreleri burada eklenebilir.
        }
    }
}
