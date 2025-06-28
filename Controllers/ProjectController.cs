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
        public async Task<ActionResult<Response<IEnumerable<ProjectDto>>>> GetProjects()
        {
            var projects = await _context.Projects
                .Include(p => p.TeamMembers)
                .Include(p => p.Tasks)
                .Where(p => !p.IsDeleted)
                .ToListAsync();

            var projectDtos = projects.Select(p => new ProjectDto
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                Department = p.Department,
                State = p.State,
                TeamMembers = p.TeamMembers.Select(u => new UserShortDto
                {
                    Id = u.Id, // Fix: Use 'User.Id' directly as it is of type 'Guid'  
                    UserName = u.UserName
                }).ToList(),
                Tasks = p.Tasks.Select(t => new TaskShortDto
                {
                    Id = t.Id, // Fix: Use 'Task.Id' directly as it is of type 'Guid'  
                    Title = t.Title
                }).ToList()
            });

            return Ok(new Response<IEnumerable<ProjectDto>>(projectDtos));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Response<Project>>> GetProject(Guid id) // Fix: Change 'id' type from 'int' to 'Guid'
        {
            var project = await _context.Projects.Include(p => p.TeamMembers).Include(p => p.Tasks)
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted); // Fix: 'p.Id' and 'id' are both of type 'Guid'
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
        public async Task<IActionResult> UpdateProject(Guid id, Project project) // Fix: Change 'id' type from 'int' to 'Guid'
        {
            if (id != project.Id) // Fix: 'id' and 'project.Id' are both of type 'Guid'
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
                if (!_context.Projects.Any(p => p.Id == id)) // Fix: 'p.Id' and 'id' are both of type 'Guid'
                    return NotFound(new Response<Project>("Project not found"));
                throw;
            }
            return Ok(new Response<Project>(project, "Project updated successfully"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(Guid id) // Fix: Change 'id' type from 'int' to 'Guid'
        {
            var project = await _context.Projects.FindAsync(id); // Fix: 'id' is now of type 'Guid'
            if (project == null)
                return NotFound(new Response<Project>("Project not found"));
            project.IsDeleted = true;
            await _context.SaveChangesAsync();
            return Ok(new Response<Project>(project, "Project deleted (soft) successfully"));
        }
    }
}
