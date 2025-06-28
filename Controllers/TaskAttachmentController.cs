using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DefineXFinalCase.Infrastructure.Data;
using DefineXFinalCase.Domain.Entities;

[ApiController]
[Route("api/[controller]")]
public class TaskAttachmentController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _env;

    public TaskAttachmentController(ApplicationDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    [HttpPost("upload")]
    public async Task<ActionResult<TaskAttachmentDto>> Upload([FromForm] TaskAttachmentUploadDto dto)
    {
        if (dto.File == null || dto.File.Length == 0)
            return BadRequest("No file uploaded.");

        if (Path.GetExtension(dto.File.FileName).ToLower() != ".pdf")
            return BadRequest("Only PDF files are allowed.");

        var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(dto.File.FileName)}";
        var filePath = Path.Combine(uploadsFolder, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await dto.File.CopyToAsync(stream);
        }

        var attachment = new TaskAttachment
        {
            FileName = dto.File.FileName,
            FilePath = $"/uploads/{fileName}",
            UploadedAt = DateTime.UtcNow,
            UserId = dto.UserId
        };

        _context.TaskAttachments.Add(attachment);
        await _context.SaveChangesAsync();

        var user = await _context.Users.FindAsync(attachment.UserId);

        var resultDto = new TaskAttachmentDto
        {
            Id = attachment.Id,
            FileName = attachment.FileName,
            FilePath = attachment.FilePath,
            UploadedAt = attachment.UploadedAt,
            User = user == null ? new UserShortDto { Id = Guid.Empty, UserName = "Unknown" } : new UserShortDto
            {
                Id = Guid.Parse(user.Id.ToString()), // Explicitly convert 'int' to 'Guid'
                UserName = user.UserName
            }
        };

        return Ok(resultDto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TaskAttachmentDto>> Get(Guid id) // Change parameter type to Guid
    {
        var attachment = await _context.TaskAttachments
            .Include(a => a.User)
            .FirstOrDefaultAsync(a => a.Id == id); // No change needed here as 'Id' is of type Guid

        if (attachment == null)
            return NotFound();

        var dto = new TaskAttachmentDto
        {
            Id = attachment.Id,
            FileName = attachment.FileName,
            FilePath = attachment.FilePath,
            UploadedAt = attachment.UploadedAt,
            User = new UserShortDto
            {
                Id = Guid.Parse(attachment.User.Id.ToString()), // Explicitly convert 'int' to 'Guid'
                UserName = attachment.User.UserName
            }
        };

        return Ok(dto);
    }
}