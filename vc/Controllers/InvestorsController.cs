using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using vc.DTOs;
using vc.Models;

[ApiController]
[Route("api/[controller]")]
public class InvestorController : ControllerBase
{
    private readonly VcdbContext _context;

    public InvestorController(VcdbContext context)
    {
        _context = context;
    }

    // ✅ Public: Get list of investors (basic info)
    [HttpGet]
    public async Task<ActionResult<IEnumerable<InvestorListDto>>> GetInvestors()
    {
        return await _context.Investors
            .Include(i => i.Country)
            .Select(i => new InvestorListDto
            {
                FullName = i.Fullname,
                InvestorType = i.Investortype,
                PublicEmail = i.Publicemail,
                CountryName = i.Country.Name,
                ProfilePhotoPath = i.Profilephotopath
            })
            .ToListAsync();
    }

    // ✅ Public: Get detailed investor profile by name (e.g., /api/investor/beelinefund)
    [HttpGet("{publicName}")]
    public async Task<ActionResult<InvestorDetailDto>> GetInvestorDetails(string publicName)
    {
        var investor = await _context.Investors
            .Include(i => i.Country)
            .Include(i => i.Industries)
            .Include(i => i.Technologies)
            .Include(i => i.Innovationmethods)
            .Include(i => i.Developmentstages)
            .FirstOrDefaultAsync(i => i.Organizationname.ToLower() == publicName.ToLower());

        if (investor == null)
            return NotFound();

        return new InvestorDetailDto
        {
            FullName = investor.Fullname,
            ContactFullName = investor.Contactfullname,
            PublicEmail = investor.Publicemail,
            PhoneNumber = investor.Phonenumber,
            CountryName = investor.Country?.Name,
            Website = investor.Website,
            OrganizationName = investor.Organizationname,
            IdentificationNumber = investor.Identificationnumber,
            Description = investor.Description,
            InvestmentAmount = investor.Investmentamount,
            HasStartupPilotExperience = investor.Hasstartuppilotexperience,
            InvestsInStartups = investor.Investsinstartups,
            ProfilePhotoPath = investor.Profilephotopath,
            LogoPath = investor.Logopath,
            Industries = investor.Industries.Select(x => x.Name).ToList(),
            Technologies = investor.Technologies.Select(x => x.Name).ToList(),
            InnovationMethods = investor.Innovationmethods.Select(x => x.Name).ToList(),
            DevelopmentStages = investor.Developmentstages.Select(x => x.Name).ToList()
        };
    }

    // ✅ Authenticated: Investor updates their profile
    [Authorize(Roles = "Investor")]
    [HttpPut]
    public async Task<IActionResult> UpdateInvestor([FromForm] InvestorAnketaDto dto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

        var investor = await _context.Investors
            .Include(i => i.Industries)
            .Include(i => i.Technologies)
            .Include(i => i.Innovationmethods)
            .Include(i => i.Developmentstages)
            .FirstOrDefaultAsync(i => i.Userid == userId);

        if (investor == null) return NotFound();

        // Update fields...
        investor.Contactfullname = dto.ContactFullName;
        investor.Publicemail = dto.PublicEmail;
        // ... and so on
        investor.Updatedat = DateTime.UtcNow;

        // Clear old and update many-to-many
        investor.Industries.Clear();
        investor.Industries = await _context.Industries.Where(i => dto.IndustryIds.Contains(i.Id)).ToListAsync();

        await _context.SaveChangesAsync();
        return Ok("Profile updated");
    }

    // ✅ Admin: Delete investor
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteInvestor(int id)
    {
        var investor = await _context.Investors.FindAsync(id);
        if (investor == null) return NotFound();

        _context.Investors.Remove(investor);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
