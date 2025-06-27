using DefineXFinalCase.Domain.Entities;
using DefineXFinalCase.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// Presentation katmanýna taþýndý. DDD için controller'lar Presentation/Controllers altýna alýnmalý.
namespace DefineXFinalCase.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public TaskController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DefineXFinalCase.Domain.Entities.Task>>> GetTasks()
        {
            return await _context.Tasks.Where(t => !t.IsDeleted).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DefineXFinalCase.Domain.Entities.Task>> GetTask(int id)
        {
            var task = await _context.Tasks
                .Include(t => t.AssignedUser)
                .Include(t => t.Comments)
                .Include(t => t.Attachments)
                .FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);
            if (task == null) return NotFound();
            return task;
        }

        [HttpPost]
        public async Task<ActionResult<DefineXFinalCase.Domain.Entities.Task>> CreateTask(DefineXFinalCase.Domain.Entities.Task task)
        {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, DefineXFinalCase.Domain.Entities.Task task)
        {
            if (id != task.Id) return BadRequest();
            _context.Entry(task).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Tasks.Any(t => t.Id == id))
                    return NotFound();
                throw;
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null) return NotFound();
            task.IsDeleted = true;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
