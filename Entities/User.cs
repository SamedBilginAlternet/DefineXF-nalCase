using System.Collections.Generic;

// Domain katmanýna taþýndý. DDD için Entities klasörü Domain/Entities olarak yeniden adlandýrýlmalý ve entity'ler burada tutulmalý.
namespace DefineXFinalCase.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; } = null!;
        public string? FullName { get; set; }
        public string Role { get; set; } = null!; // PM, TL, Member
        public ICollection<Project> Projects { get; set; } = new List<Project>();
        public ICollection<Task> AssignedTasks { get; set; } = new List<Task>(); // Many-to-many
    }
}
