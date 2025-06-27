using DefineXFinalCase.Domain.Entities;
using DefineXFinalCase.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DefineXFinalCase;
using TaskEntity = DefineXFinalCase.Domain.Entities.Task;
using Microsoft.AspNetCore.Authorization;

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
        [Authorize]
        public async System.Threading.Tasks.Task<ActionResult<Response<IEnumerable<TaskEntity>>>> GetTasks()
        {
            var tasks = await _context.Tasks.Where(t => !t.IsDeleted).ToListAsync();
            return Ok(new Response<IEnumerable<TaskEntity>>(tasks));
        }
    }
}
