public class UserDto
{
    public int Id { get; set; }
    public string UserName { get; set; } = null!;
    public string? FullName { get; set; }
    public string Role { get; set; } = null!;
    public List<ProjectShortDto>? Projects { get; set; }
    public List<TaskShortDto>? AssignedTasks { get; set; }
}