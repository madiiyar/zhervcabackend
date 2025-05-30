using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vc.DTOs;
using vc.Models;

namespace vc.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/admin")]
    public class AdminController : ControllerBase
    {
        private readonly VcdbContext _context;

        public AdminController(VcdbContext context)
        {
            _context = context;
        }

        // Get all startups that are pending approval
        [HttpGet("startups/pending")]
        public async Task<ActionResult<IEnumerable<StartupListDto>>> GetPendingStartups()
        {
            return await _context.Startups
                .Include(s => s.Country)
                .Include(s => s.Developmentstage)
                .Where(s => s.Status == "Pending")
                .Select(s => new StartupListDto
                {
                    PublicName = s.Publicname,
                    CountryName = s.Country!.Name,
                    DevelopmentStage = s.Developmentstage!.Name,
                    LogoPath = s.Logopath
                })
                .ToListAsync();
        }

        //Approve or reject a startup
        [HttpPut("startups/{id}/status")]
        public async Task<IActionResult> UpdateStartupStatus(int id, [FromBody] UpdateStartupStatusDto dto)
        {
            var startup = await _context.Startups.FindAsync(id);
            if (startup == null)
                return NotFound("Startup not found.");

            var allowedStatuses = new[] { "Accepted", "Rejected" };
            if (!allowedStatuses.Contains(dto.Status))
                return BadRequest("Status must be either 'Accepted' or 'Rejected'.");

            startup.Status = dto.Status;
            startup.Updatedat = DateTime.Now;

            await _context.SaveChangesAsync();
            return Ok($"Startup status updated to '{dto.Status}'.");
        }

        // View all users (for auditing)
        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _context.Users
                .Select(u => new
                {
                    u.Id,
                    u.Fullname,
                    u.Email,
                    u.Role,
                    u.Isemailconfirmed
                })
                .ToListAsync();

            return Ok(users);
        }
    }
}
