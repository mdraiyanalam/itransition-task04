using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using task04UserManagement.Models;
using task04UserManagement.Data;

namespace task04UserManagement.Controllers
{
    [Route("api/users")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/users - Sorted by Last Login
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await _context.Users
                .OrderByDescending(u => u.LastLoginTime)
                .ToListAsync();

            return Ok(users);
        }

        // Block users
        [HttpPost("block")]
        public async Task<IActionResult> BlockUsers([FromBody] List<int> userIds)
        {
            var users = await _context.Users.Where(u => userIds.Contains(u.Id)).ToListAsync();
            foreach (var user in users)
            {
                user.Status = "Blocked";
            }
            await _context.SaveChangesAsync();
            return Ok(new { message = "Users blocked successfully" });
        }

        // Unblock users
        [HttpPost("unblock")]
        public async Task<IActionResult> UnblockUsers([FromBody] List<int> userIds)
        {
            var users = await _context.Users.Where(u => userIds.Contains(u.Id)).ToListAsync();
            foreach (var user in users)
            {
                if (user.Status == "Blocked")
                    user.Status = "Active";
            }
            await _context.SaveChangesAsync();
            return Ok(new { message = "Users unblocked successfully" });
        }

        // Delete users (hard delete)
        [HttpPost("delete")]
        public async Task<IActionResult> DeleteUsers([FromBody] List<int> userIds)
        {
            var users = await _context.Users.Where(u => userIds.Contains(u.Id)).ToListAsync();
            _context.Users.RemoveRange(users);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Users deleted successfully" });
        }

        // Delete unverified users
        [HttpPost("delete-unverified")]
        public async Task<IActionResult> DeleteUnverified()
        {
            var unverified = await _context.Users.Where(u => u.Status == "Unverified").ToListAsync();
            _context.Users.RemoveRange(unverified);
            await _context.SaveChangesAsync();
            return Ok(new { message = $"{unverified.Count} unverified users deleted" });
        }
    }
}