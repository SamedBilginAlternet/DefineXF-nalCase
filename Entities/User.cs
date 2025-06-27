using System.Collections.Generic;

namespace DefineXFinalCase.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; } = null!;
        public string? FullName { get; set; }
        public string Role { get; set; } = null!; // PM, TL, Member
        public ICollection<Project> Projects { get; set; } = new List<Project>();
        public ICollection<Task> AssignedTasks { get; set; } = new List<Task>();
    }
}
