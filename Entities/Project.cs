using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DefineXFinalCase.Domain.Entities
{
    public class Project
    {
        public int Id { get; set; }
        [Required, StringLength(100)]
        public string Title { get; set; } = null!;
        [Required, StringLength(500)]
        public string Description { get; set; } = null!;
        [Required, StringLength(100)]
        public string Department { get; set; } = null!;
        [Required]
        public string State { get; set; } = "In Progress"; // In Progress, Cancelled, Completed
        public ICollection<User> TeamMembers { get; set; } = new List<User>();
        public ICollection<Task> Tasks { get; set; } = new List<Task>();
        public bool IsDeleted { get; set; } = false; // Soft delete
    }
}
