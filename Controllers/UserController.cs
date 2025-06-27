using DefineXFinalCase.Domain.Entities;
using DefineXFinalCase.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DefineXFinalCase;
using Microsoft.AspNetCore.Authorization;

namespace DefineXFinalCase.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<Response<IEnumerable<User>>>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(new Response<IEnumerable<User>>(users));
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Response<User>>> GetUser(int id)
        {
            var user = await _context.Users.Include(u => u.Projects).Include(u => u.AssignedTasks)
                .FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
                return NotFound(new Response<User>("User not found"));
            return Ok(new Response<User>(user));
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Response<User>>> CreateUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, new Response<User>(user));
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateUser(int id, User user)
        {
            if (id != user.Id)
                return BadRequest(new Response<User>("Id mismatch"));
            _context.Entry(user).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Users.Any(u => u.Id == id))
                    return NotFound(new Response<User>("User not found"));
                throw;
            }
            return Ok(new Response<User>(user, "User updated successfully"));
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound(new Response<User>("User not found"));
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return Ok(new Response<User>(user, "User deleted successfully"));
        }
    }
}
