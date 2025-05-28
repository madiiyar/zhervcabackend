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

    [HttpPost]
    public async Task<IActionResult> CreateInvestor([FromForm] InvestorAnketaDto dto)
    {
        // For now, hardcode user ID for testing (or pull from JWT later)
        var userId = 3;

        var investor = new Investor
        {
            Userid = userId,
            Investortype = "Angel", // or from UI if you plan to support 'Fund'
            Fullname = dto.FullName,
            Contactfullname = dto.ContactFullName,
            Publicemail = dto.PublicEmail,
            Phonenumber = dto.PhoneNumber,
            Countryid = dto.CountryId,
            Website = dto.Website,
            Organizationname = dto.OrganizationName,
            Identificationnumber = dto.IdentificationNumber,
            Description = dto.Description,
            Investmentamount = dto.InvestmentAmount,
            Hasstartuppilotexperience = dto.HasStartupPilotExperience,
            Investsinstartups = dto.InvestsInStartups,
            Sourceinfoid = dto.SourceInfoId,
            Createdat = DateTime.Now,
            Updatedat = DateTime.Now
        };

        // ✅ Handle profile photo upload
        if (dto.ProfilePhoto != null)
        {
            var uploadsDir = Path.Combine("wwwroot", "uploads");
            if (!Directory.Exists(uploadsDir))
                Directory.CreateDirectory(uploadsDir);

            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(dto.ProfilePhoto.FileName);
            var filePath = Path.Combine(uploadsDir, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await dto.ProfilePhoto.CopyToAsync(stream);
            }

            investor.Profilephotopath = "/uploads/" + uniqueFileName;
        }

        // Save the base investor row
        _context.Investors.Add(investor);
        await _context.SaveChangesAsync();

        // ✅ Many-to-many relations (after investor.Id is generated)
        investor.Industries = await _context.Industries.Where(i => dto.IndustryIds.Contains(i.Id)).ToListAsync();
        investor.Technologies = await _context.Technologies.Where(t => dto.TechnologyIds.Contains(t.Id)).ToListAsync();
        investor.Innovationmethods = await _context.Innovationmethods.Where(m => dto.InnovationMethodIds.Contains(m.Id)).ToListAsync();
        investor.Developmentstages = await _context.Developmentstages.Where(d => dto.DevelopmentStageIds.Contains(d.Id)).ToListAsync();

        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetInvestorDetails), new { publicName = investor.Organizationname }, "Investor profile created.");
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
