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
                    Id = s.Id,
                    OrganizationName = s.Organizationname,
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


        // GET: api/admin/startups — Full list with user info
        [HttpGet("startups")]
        public async Task<IActionResult> GetAllStartups()
        {
            var startups = await _context.Startups
                .Include(s => s.User)
                .Include(s => s.Country)
                .Include(s => s.Developmentstage)
                .Include(s => s.Investmentstage)
                .Select(s => new
                {
                    s.Id,
                    s.Publicname,
                    s.Organizationname,
                    s.Status,
                    s.Createdat,
                    s.Updatedat,
                    User = new
                    {
                        s.User.Id,
                        s.User.Fullname,
                        s.User.Email,
                        s.User.Role
                    },
                    Country = s.Country != null ? s.Country.Name : null,
                    DevelopmentStage = s.Developmentstage != null ? s.Developmentstage.Name : null,
                    InvestmentStage = s.Investmentstage != null ? s.Investmentstage.Name : null
                })
                .ToListAsync();

            return Ok(startups);
        }

        // GET: api/admin/investors — Full list with user info
        [HttpGet("investors")]
        public async Task<IActionResult> GetAllInvestors()
        {
            var investors = await _context.Investors
                .Include(i => i.User)
                .Include(i => i.Country)
                .Select(i => new
                {
                    i.Id,
                    i.Fullname,
                    i.Organizationname,
                    i.Investortype,
                    i.Createdat,
                    i.Updatedat,
                    User = i.User == null ? null : new
                    {
                        i.User.Id,
                        i.User.Fullname,
                        i.User.Email,
                        i.User.Role
                    },
                    Country = i.Country != null ? i.Country.Name : null
                })
                .ToListAsync();

            return Ok(investors);
        }

        // GET: api/admin/offers — Full list with related entities
        [HttpGet("offers")]
        public async Task<IActionResult> GetAllOffers()
        {
            var offers = await _context.Startupinvestoroffers
                .Include(o => o.Startup)
                .ThenInclude(s => s.User)
                .Include(o => o.Investor)
                .ThenInclude(i => i.User)
                .Select(o => new
                {
                    o.Id,
                    o.Status,
                    o.Requestedby,
                    o.Message,
                    o.Createdat,
                    o.Updatedat,
                    Startup = o.Startup == null ? null : new
                    {
                        o.Startup.Id,
                        o.Startup.Publicname,
                        User = o.Startup.User == null ? null : new
                        {
                            o.Startup.User.Id,
                            o.Startup.User.Fullname,
                            o.Startup.User.Email,
                            o.Startup.User.Role
                        }
                    },
                    Investor = o.Investor == null ? null : new
                    {
                        o.Investor.Id,
                        o.Investor.Organizationname,
                        o.Investor.Investortype,
                        User = o.Investor.User == null ? null : new
                        {
                            o.Investor.User.Id,
                            o.Investor.User.Fullname,
                            o.Investor.User.Email,
                            o.Investor.User.Role
                        }
                    }
                })
                .ToListAsync();

            return Ok(offers);
        }
    }
}
