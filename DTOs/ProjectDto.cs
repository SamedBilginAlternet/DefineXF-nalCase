public class ProjectDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Department { get; set; } = null!;
    public string State { get; set; } = null!;
    public List<UserShortDto>? TeamMembers { get; set; }
    public List<TaskShortDto>? Tasks { get; set; }
}