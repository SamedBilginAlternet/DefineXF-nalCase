using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DefineXFinalCase.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = null!;
        public string? FullName { get; set; }
        public string Role { get; set; } = null!; // PM, TL, Member
        public ICollection<Project> Projects { get; set; } = new List<Project>();
        public ICollection<Task> AssignedTasks { get; set; } = new List<Task>(); // Many-to-many
    }
}
