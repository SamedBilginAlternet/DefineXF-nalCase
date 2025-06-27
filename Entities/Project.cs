using System.Collections.Generic;

namespace DefineXFinalCase.Domain.Entities
{
    public class Project
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Department { get; set; } = null!;
        public string State { get; set; } = "In Progress"; // In Progress, Cancelled, Completed
        public ICollection<User> TeamMembers { get; set; } = new List<User>();
        public ICollection<Task> Tasks { get; set; } = new List<Task>();
        public bool IsDeleted { get; set; } = false; // Soft delete
    }
}
