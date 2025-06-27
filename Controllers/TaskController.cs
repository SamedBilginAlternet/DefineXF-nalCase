using DefineXFinalCase.Domain.Entities;
using DefineXFinalCase.Domain.Enums;
using DefineXFinalCase.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskEntity = DefineXFinalCase.Domain.Entities.Task;

namespace DefineXFinalCase.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TaskController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public TaskController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/task
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<Response<IEnumerable<TaskEntity>>>> GetTasks()
    {
        var tasks = await _context.Tasks
            .Include(t => t.Comments)
            .Include(t => t.Attachments)
            .ToListAsync();

        return Ok(new Response<IEnumerable<TaskEntity>>(tasks));
    }

    // PUT: api/task/{id}
    // G�revi g�ncelle (rol kontrol� ile)
    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskEntity updatedTask)
    {
        var existingTask = await _context.Tasks
            .Include(t => t.Attachments)
            .Include(t => t.Comments)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (existingTask == null)
            return NotFound();

        // Rol� al
        var role = User.FindFirstValue(ClaimTypes.Role);

        // ?? E�er kullan�c� Team Member ise title ve description de�i�tiremesin
        if (role == "TeamMember")
        {
            updatedTask.Title = existingTask.Title;
            updatedTask.Description = existingTask.Description;
        }

        // ? Completed ise ba�ka state'e ge�emez
        if (existingTask.State == TaskState.Completed && updatedTask.State != TaskState.Completed)
        {
            return BadRequest("Completed g�rev ba�ka bir duruma ge�irilemez.");
        }

        // ? Cancelled veya Blocked ise Reason zorunlu
        if ((updatedTask.State == TaskState.Cancelled || updatedTask.State == TaskState.Blocked)
            && string.IsNullOrWhiteSpace(updatedTask.Reason))
        {
            return BadRequest("Cancelled veya Blocked i�in a��klama (reason) girilmelidir.");
        }

        // G�ncelle
        existingTask.State = updatedTask.State;
        existingTask.Reason = updatedTask.Reason;
        existingTask.Attachments = updatedTask.Attachments; // iste�e g�re kontrolle s�n�rland�r�labilir

        await _context.SaveChangesAsync();
        return NoContent();
    }
}
