using DefineXFinalCase.Domain.Entities;
using DefineXFinalCase.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DefineXFinalCase;

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
        public async Task<ActionResult<Response<IEnumerable<DefineXFinalCase.Domain.Entities.Task>>>> GetTasks()
        {
            var tasks = await _context.Tasks.Where(t => !t.IsDeleted).ToListAsync();
            return Ok(new Response<IEnumerable<DefineXFinalCase.Domain.Entities.Task>>(tasks));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Response<DefineXFinalCase.Domain.Entities.Task>>> GetTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null || task.IsDeleted)
            {
                return NotFound(new Response<DefineXFinalCase.Domain.Entities.Task>("Task not found"));
            }
            return Ok(new Response<DefineXFinalCase.Domain.Entities.Task>(task));
        }

        [HttpPost]
        public async Task<ActionResult<Response<Task>>> CreateTask(Task task)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new Response<Task>("Validation error", errors));
            }
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTask), new { id = task.Id }, new Response<Task>(task));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, Task task)
        {
            if (id != task.Id)
                return BadRequest(new Response<Task>("Id mismatch"));
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new Response<Task>("Validation error", errors));
            }
            _context.Entry(task).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Tasks.Any(t => t.Id == id))
                    return NotFound(new Response<Task>("Task not found"));
                throw;
            }
            return Ok(new Response<Task>(task, "Task updated successfully"));
        }
    }
}
