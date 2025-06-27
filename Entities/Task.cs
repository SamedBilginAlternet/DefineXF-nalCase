using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DefineXFinalCase.Domain.Enums;

namespace DefineXFinalCase.Domain.Entities
{
    public class Task
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Title { get; set; } = null!;

        [Required, StringLength(500)]
        public string Description { get; set; } = null!; // User story

        [Required, StringLength(500)]
        public string AcceptanceCriteria { get; set; } = null!;

        [Required]
        public TaskState State { get; set; } = TaskState.Backlog;

        [Required]
        public TaskPriority Priority { get; set; } = TaskPriority.Medium;

        public int? AssignedUserId { get; set; }
        public User? AssignedUser { get; set; }

        public ICollection<User> AssignedUsers { get; set; } = new List<User>();

        public int ProjectId { get; set; }
        public Project Project { get; set; } = null!;

        public ICollection<TaskComment> Comments { get; set; } = new List<TaskComment>();
        public ICollection<TaskAttachment> Attachments { get; set; } = new List<TaskAttachment>();

        public string? Reason { get; set; } // Cancelled veya Blocked için gerekli açýklama

        public bool IsDeleted { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
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
