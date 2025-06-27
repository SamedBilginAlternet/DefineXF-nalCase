// Presentation katmanýna taþýndý. DDD için controller'lar Presentation/Controllers altýna alýnmalý.

using DefineXFinalCase.Domain.Entities;
using DefineXFinalCase.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DefineXFinalCase;

namespace DefineXFinalCase.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public ProjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<Response<IEnumerable<Project>>>> GetProjects()
        {
            var projects = await _context.Projects.Where(p => !p.IsDeleted).ToListAsync();
            return Ok(new Response<IEnumerable<Project>>(projects));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Response<Project>>> GetProject(int id)
        {
            var project = await _context.Projects.Include(p => p.TeamMembers).Include(p => p.Tasks)
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
            if (project == null)
                return NotFound(new Response<Project>("Project not found"));
            return Ok(new Response<Project>(project));
        }

        [HttpPost]
        public async Task<ActionResult<Response<Project>>> CreateProject(Project project)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new Response<Project>("Validation error", errors));
            }
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProject), new { id = project.Id }, new Response<Project>(project));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(int id, Project project)
        {
            if (id != project.Id)
                return BadRequest(new Response<Project>("Id mismatch"));
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new Response<Project>("Validation error", errors));
            }
            _context.Entry(project).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Projects.Any(p => p.Id == id))
                    return NotFound(new Response<Project>("Project not found"));
                throw;
            }
            return Ok(new Response<Project>(project, "Project updated successfully"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
                return NotFound(new Response<Project>("Project not found"));
            project.IsDeleted = true;
            await _context.SaveChangesAsync();
            return Ok(new Response<Project>(project, "Project deleted (soft) successfully"));
        }
    }
}
