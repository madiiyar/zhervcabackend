using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using vc.DTOs;
using vc.Models;

namespace vc.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class OffersController : ControllerBase
    {
        private readonly VcdbContext _context;

        public OffersController(VcdbContext context)
        {
            _context = context;
        }

        //  Create a new offer
        [HttpPost]
        public async Task<IActionResult> CreateOffer([FromBody] CreateOfferDto dto)
        {
            var userIdStr = User.FindFirst("id")?.Value;
            if (!int.TryParse(userIdStr, out var userid))
                return Unauthorized("Invalid user ID.");

            var role = User.Claims.FirstOrDefault(c => c.Type.Contains("role"))?.Value;


            int? startupId = null;
            int? investorId = null;

            if (role == "Startup")
            {
                startupId = await _context.Startups
                    .Where(s => s.Userid == userid)
                    .Select(s => (int?)s.Id)
                    .FirstOrDefaultAsync();
                investorId = dto.TargetId;
            }
            else if (role == "Investor")
            {
                investorId = await _context.Investors
                    .Where(i => i.Userid == userid)
                    .Select(i => (int?)i.Id)
                    .FirstOrDefaultAsync();
                startupId = dto.TargetId;
            }
            else
            {
                return Unauthorized("Only startups or investors can send offers.");
            }

            if (startupId == null || investorId == null)
                return BadRequest("Invalid target or sender profile.");

            var exists = await _context.Startupinvestoroffers.AnyAsync(o =>
                o.Startupid == startupId &&
                o.Investorid == investorId &&
                o.Status == "Pending");

            if (exists)
                return Conflict("Pending offer already exists.");

            var offer = new Startupinvestoroffer
            {
                Startupid = startupId,
                Investorid = investorId,
                Requestedby = role,
                Status = "Pending",
                Message = dto.Message,
                Createdat = DateTime.Now,
                Updatedat = DateTime.Now
            };

            _context.Startupinvestoroffers.Add(offer);
            await _context.SaveChangesAsync();

            return Ok("Offer sent successfully.");
        }

        //Get sent offers
        [HttpGet("sent")]
        public async Task<ActionResult<IEnumerable<OfferResponseDto>>> GetSentOffers()
        {
            var userIdStr = User.FindFirst("id")?.Value;
            if (!int.TryParse(userIdStr, out var userId))
                return Unauthorized("Invalid or missing user ID.");

            var role = User.Claims.FirstOrDefault(c => c.Type.Contains("role"))?.Value;
            if (string.IsNullOrEmpty(role))
                return Unauthorized("Role claim missing.");


            if (role == "Startup")
            {
                var startupId = await _context.Startups
                    .Where(s => s.Userid == userId)
                    .Select(s => s.Id)
                    .FirstOrDefaultAsync();

                return await _context.Startupinvestoroffers
                    .Include(o => o.Startup)
                    .Include(o => o.Investor)
                    .Where(o => o.Startupid == startupId && o.Requestedby == "Startup")
                    .Select(o => new OfferResponseDto
                    {
                        Id = o.Id,
                        StartupName = o.Startup.Publicname,
                        InvestorName = o.Investor.Organizationname,
                        RequestedBy = o.Requestedby,
                        Status = o.Status,
                        Message = o.Message,
                        CreatedAt = o.Createdat.Value,
                        UpdatedAt = o.Updatedat.Value
                    })
                    .ToListAsync();
            }
            else if (role == "Investor")
            {
                var investorId = await _context.Investors
                    .Where(i => i.Userid == userId)
                    .Select(i => i.Id)
                    .FirstOrDefaultAsync();

                return await _context.Startupinvestoroffers
                    .Include(o => o.Startup)
                    .Include(o => o.Investor)
                    .Where(o => o.Investorid == investorId && o.Requestedby == "Investor")
                    .Select(o => new OfferResponseDto
                    {
                        Id = o.Id,
                        StartupName = o.Startup.Publicname,
                        InvestorName = o.Investor.Organizationname,
                        RequestedBy = o.Requestedby,
                        Status = o.Status,
                        Message = o.Message,
                        CreatedAt = o.Createdat.Value,
                        UpdatedAt = o.Updatedat.Value
                    })
                    .ToListAsync();
            }

            return Unauthorized();
        }

        //Get received offers
        [HttpGet("received")]
        public async Task<ActionResult<IEnumerable<OfferResponseDto>>> GetReceivedOffers()
        {
            var userIdStr = User.FindFirst("id")?.Value;
            if (!int.TryParse(userIdStr, out var userId))
                return Unauthorized("Invalid or missing user ID.");

            var role = User.Claims.FirstOrDefault(c => c.Type.Contains("role"))?.Value;
            if (string.IsNullOrEmpty(role))
                return Unauthorized("Role claim missing.");


            if (role == "Startup")
            {
                var startupId = await _context.Startups
                    .Where(s => s.Userid == userId)
                    .Select(s => s.Id)
                    .FirstOrDefaultAsync();

                return await _context.Startupinvestoroffers
                    .Include(o => o.Startup)
                    .Include(o => o.Investor)
                    .Where(o => o.Startupid == startupId && o.Requestedby == "Investor")
                    .Select(o => new OfferResponseDto
                    {
                        Id = o.Id,
                        StartupName = o.Startup.Publicname,
                        InvestorName = o.Investor.Organizationname,
                        RequestedBy = o.Requestedby,
                        Status = o.Status,
                        Message = o.Message,
                        CreatedAt = o.Createdat.Value,
                        UpdatedAt = o.Updatedat.Value
                    })
                    .ToListAsync();
            }
            else if (role == "Investor")
            {
                var investorId = await _context.Investors
                    .Where(i => i.Userid == userId)
                    .Select(i => i.Id)
                    .FirstOrDefaultAsync();

                return await _context.Startupinvestoroffers
                    .Include(o => o.Startup)
                    .Include(o => o.Investor)
                    .Where(o => o.Investorid == investorId && o.Requestedby == "Startup")
                    .Select(o => new OfferResponseDto
                    {
                        Id = o.Id,
                        StartupName = o.Startup.Publicname,
                        InvestorName = o.Investor.Organizationname,
                        RequestedBy = o.Requestedby,
                        Status = o.Status,
                        Message = o.Message,
                        CreatedAt = o.Createdat.Value,
                        UpdatedAt = o.Updatedat.Value
                    })
                    .ToListAsync();
            }

            return Unauthorized();
        }

        //Update offer status
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOffer(int id, [FromBody] UpdateOfferStatusDto dto)
        {
            var userIdStr = User.FindFirst("id")?.Value;
            if (!int.TryParse(userIdStr, out var userId))
                return Unauthorized("Invalid or missing user ID.");

            var role = User.Claims.FirstOrDefault(c => c.Type.Contains("role"))?.Value;
            if (string.IsNullOrEmpty(role))
                return Unauthorized("Role claim missing.");


            var offer = await _context.Startupinvestoroffers
                .Include(o => o.Startup)
                .Include(o => o.Investor)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (offer == null)
                return NotFound();

            if (role == "Startup" && offer.Startup.Userid != userId && offer.Requestedby != "Startup")
                return Forbid();

            if (role == "Investor" && offer.Investor.Userid != userId && offer.Requestedby != "Investor")
                return Forbid();

            if (dto.Status is not ("Accepted" or "Rejected" or "Cancelled"))
                return BadRequest("Invalid status.");

            offer.Status = dto.Status;
            offer.Updatedat = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return Ok("Offer updated.");
        }
    }
}
