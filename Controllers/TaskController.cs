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
            var task = await _context.Tasks
               
