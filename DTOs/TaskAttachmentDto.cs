public class TaskAttachmentDto
{
    public int Id { get; set; }
    public string FileName { get; set; } = null!;
    public string FilePath { get; set; } = null!;
    public DateTime UploadedAt { get; set; }
    public UserShortDto User { get; set; } = null!;
}