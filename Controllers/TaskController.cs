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
    // Görevi güncelle (rol kontrolü ile)
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

        // Rolü al
        var role = User.FindFirstValue(ClaimTypes.Role);

        // ?? Eðer kullanýcý Team Member ise title ve description deðiþtiremesin
        if (role == "TeamMember")
        {
            updatedTask.Title = existingTask.Title;
            updatedTask.Description = existingTask.Description;
        }

        // ? Completed ise baþka state'e geçemez
        if (existingTask.State == TaskState.Completed && updatedTask.State != TaskState.Completed)
        {
            return BadRequest("Completed görev baþka bir duruma geçirilemez.");
        }

        // ? Cancelled veya Blocked ise Reason zorunlu
        if ((updatedTask.State == TaskState.Cancelled || updatedTask.State == TaskState.Blocked)
            && string.IsNullOrWhiteSpace(updatedTask.Reason))
        {
            return BadRequest("Cancelled veya Blocked için açýklama (reason) girilmelidir.");
        }

        // Güncelle
        existingTask.State = updatedTask.State;
        existingTask.Reason = updatedTask.Reason;
        existingTask.Attachments = updatedTask.Attachments; // isteðe göre kontrolle sýnýrlandýrýlabilir

        await _context.SaveChangesAsync();
        return NoContent();
    }
}
