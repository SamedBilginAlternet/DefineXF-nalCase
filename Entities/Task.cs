using System;
using System.Collections.Generic;

namespace DefineXFinalCase.Domain.Entities
{
    public class Task
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!; // User story
        public string AcceptanceCriteria { get; set; } = null!;
        public string State { get; set; } = "Backlog"; // Backlog, In Analysis, In Development, Blocked, Cancelled, Completed
        public string Priority { get; set; } = "Medium"; // Critical, High, Medium, Low
        public int? AssignedUserId { get; set; }
        public User? AssignedUser { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; } = null!;
        public ICollection<TaskComment> Comments { get; set; } = new List<TaskComment>();
        public ICollection<TaskAttachment> Attachments { get; set; } = new List<TaskAttachment>();
        public bool IsDeleted { get; set; } = false; // Soft delete
        public string? BlockedOrCancelledReason { get; set; }
    }

    public class TaskComment
    {
        public int Id { get; set; }
        public string Comment { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }

    public class TaskAttachment
    {
        public int Id { get; set; }
        public string FileName { get; set; } = null!;
        public string FilePath { get; set; } = null!;
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
