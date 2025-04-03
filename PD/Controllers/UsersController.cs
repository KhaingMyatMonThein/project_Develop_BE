using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PD.Data;
using PD.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BCrypt.Net;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers([FromQuery] int page = 1, 
            [FromQuery] int pageSize = 10)
        {
            var users = await _context.Users
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound(new { message = "User not found" });

            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<User>> CreateUser([FromBody] UserRequest request)
        {
            if (request == null || request.User == null)
            {
                return BadRequest(new { message = "User data is required" });
            }

            if (await _context.Users.AnyAsync(u => u.Email == request.User.Email))
            {
                return BadRequest(new { message = "Email already exists" });
            }

            // Hash the password
            request.User.Password = BCrypt.Net.BCrypt.HashPassword(request.User.Password);

            _context.Users.Add(request.User);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = request.User.Id }, request.User);
        }

        // PUT: api/users/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserRequest request)
        {
            if (request == null || request.User == null)
            {
                return BadRequest(new { message = "User data is required" });
            }

            if (id != request.User.Id)
            {
                return BadRequest(new { message = "Invalid user ID" });
            }

            var existingUser = await _context.Users.FindAsync(id);
            if (existingUser == null)
            {
                return NotFound(new { message = "User not found" });
            }

            if (existingUser.Email != request.User.Email &&
                await _context.Users.AnyAsync(u => u.Email == request.User.Email))
            {
                return BadRequest(new { message = "Email already exists" });
            }

            existingUser.Name = request.User.Name;
            existingUser.Email = request.User.Email;
            existingUser.Role = request.User.Role;

            if (!string.IsNullOrEmpty(request.User.Password))
            {
                existingUser.Password = BCrypt.Net.BCrypt.HashPassword(request.User.Password);
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/users/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

  
}
