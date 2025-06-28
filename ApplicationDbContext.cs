using Microsoft.EntityFrameworkCore;
using DefineXFinalCase.Domain.Entities;
using TaskEntity = DefineXFinalCase.Domain.Entities.Task;

namespace DefineXFinalCase.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Project> Projects => Set<Project>();
        public DbSet<TaskEntity> Tasks => Set<TaskEntity>();
        public DbSet<TaskComment> TaskComments => Set<TaskComment>();
        public DbSet<TaskAttachment> TaskAttachments => Set<TaskAttachment>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Task <-> User many-to-many
            modelBuilder.Entity<TaskEntity>()
                .HasMany(t => t.AssignedUsers)
                .WithMany(u => u.AssignedTasks)
                .UsingEntity(j => j.ToTable("TaskAssignedUsers"));

            // Task <-> User one-to-many (AssignedUser)
            modelBuilder.Entity<TaskEntity>()
                .HasOne(t => t.AssignedUser)
                .WithMany()
                .HasForeignKey(t => t.AssignedUserId)
                .OnDelete(DeleteBehavior.SetNull);

            // Soft delete global filter
            modelBuilder.Entity<Project>().HasQueryFilter(p => !p.IsDeleted);
            modelBuilder.Entity<TaskEntity>().HasQueryFilter(t => !t.IsDeleted);
        }
    }
}