public class TaskAttachmentUploadDto
{
    public Guid TaskId { get; set; }
    public Guid UserId { get; set; }
    public IFormFile File { get; set; } = null!;
}