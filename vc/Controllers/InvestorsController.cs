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

    // Helper property to get the user ID from the "id" claim
    private int? UserId
    {
        get
        {
            var claim = User.FindFirst("id");
            if (claim == null) return null;
            if (int.TryParse(claim.Value, out var id)) return id;
            return null;
        }
    }

    //  Public: Get list of investors (full details  info)
    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<InvestorListDto>>> GetInvestors()
    {
        return await _context.Investors
                .Include(i => i.Country)
                .Select(i => new InvestorListDto
                {
                    Id = i.Id,
                    FullName = i.Fullname,
                    OrganizationName = i.Organizationname,
                    InvestorType = i.Investortype,
                    PublicEmail = i.Publicemail,
                    CountryName = i.Country.Name,
                    ProfilePhotoPath = i.Profilephotopath
                })
                .ToListAsync();
    }

    // ✅ Public: Get detailed investor profile by name (e.g., /api/investor/beelinefund)
    [AllowAnonymous]
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
            Id=investor.Id,
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

    [Authorize(Roles = "Investor")]
    [HttpGet("me")]
    public async Task<ActionResult<InvestorDetailDto>> GetMyInvestor()
    {
        var userIdClaim = User.FindFirst("id");
        if (userIdClaim == null) return Unauthorized("User ID claim missing");

        var userId = int.Parse(userIdClaim.Value);

        var investor = await _context.Investors
            .Include(i => i.Country)
            .Include(i => i.Industries)
            .Include(i => i.Technologies)
            .Include(i => i.Innovationmethods)
            .Include(i => i.Developmentstages)
            .FirstOrDefaultAsync(i => i.Userid == userId);

        if (investor == null) return NotFound("Investor profile not found");

        return new InvestorDetailDto
        {
            Id = investor.Id,
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



    // Create investor profile - authenticated user only
    [Authorize(Roles = "Investor")]
    [HttpPost]
    public async Task<IActionResult> CreateInvestor([FromForm] InvestorAnketaDto dto)
    {
        var userIdClaim = User.FindFirst("id");
        if (userIdClaim == null) return Unauthorized("User ID claim missing");

        var userId = int.Parse(userIdClaim.Value);

        var investor = new Investor
        {
            Userid = userId,
            Investortype = dto.InvestorType,
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

        if (dto.ProfilePhoto != null)
        {
            var uploadsDir = Path.Combine("wwwroot", "uploads");
            if (!Directory.Exists(uploadsDir)) Directory.CreateDirectory(uploadsDir);

            var uniqueFileName = Guid.NewGuid() + Path.GetExtension(dto.ProfilePhoto.FileName);
            var filePath = Path.Combine(uploadsDir, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await dto.ProfilePhoto.CopyToAsync(stream);
            }

            investor.Profilephotopath = "/uploads/" + uniqueFileName;
        }

        if (dto.Logo != null)
        {
            var uploadsDir = Path.Combine("wwwroot", "uploads");
            if (!Directory.Exists(uploadsDir))
                Directory.CreateDirectory(uploadsDir);

            var uniqueFileName = Guid.NewGuid() + Path.GetExtension(dto.Logo.FileName);
            var filePath = Path.Combine(uploadsDir, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await dto.Logo.CopyToAsync(stream);
            }

            investor.Logopath = "/uploads/" + uniqueFileName;
        }


        _context.Investors.Add(investor);
        await _context.SaveChangesAsync();

        investor.Industries = await _context.Industries.Where(i => dto.IndustryIds.Contains(i.Id)).ToListAsync();
        investor.Technologies = await _context.Technologies.Where(t => dto.TechnologyIds.Contains(t.Id)).ToListAsync();
        investor.Innovationmethods = await _context.Innovationmethods.Where(m => dto.InnovationMethodIds.Contains(m.Id)).ToListAsync();
        investor.Developmentstages = await _context.Developmentstages.Where(d => dto.DevelopmentStageIds.Contains(d.Id)).ToListAsync();

        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetInvestors), new { publicName = investor.Organizationname }, "Investor profile created.");
    }

    // Update investor profile - authenticated only
    [Authorize(Roles = "Investor")]
    [HttpPut]
    public async Task<IActionResult> UpdateInvestor([FromForm] InvestorAnketaDto dto)
    {
        var userIdClaim = User.FindFirst("id");
        if (userIdClaim == null) return Unauthorized("User ID claim missing");

        var userId = int.Parse(userIdClaim.Value);

        // Load investor with navigation properties
        var investor = await _context.Investors
            .Include(i => i.Industries)
            .Include(i => i.Technologies)
            .Include(i => i.Innovationmethods)
            .Include(i => i.Developmentstages)
            .FirstOrDefaultAsync(i => i.Userid == userId);

        if (investor == null)
            return NotFound("Investor profile not found");

        // Update scalar fields
        investor.Fullname = dto.FullName;
        investor.Contactfullname = dto.ContactFullName;
        investor.Publicemail = dto.PublicEmail;
        investor.Phonenumber = dto.PhoneNumber;
        investor.Countryid = dto.CountryId;
        investor.Website = dto.Website;
        investor.Organizationname = dto.OrganizationName;
        investor.Investortype = dto.InvestorType;
        investor.Identificationnumber = dto.IdentificationNumber;
        investor.Description = dto.Description;
        investor.Investmentamount = dto.InvestmentAmount;
        investor.Hasstartuppilotexperience = dto.HasStartupPilotExperience;
        investor.Investsinstartups = dto.InvestsInStartups;
        investor.Sourceinfoid = dto.SourceInfoId;
        investor.Updatedat = DateTime.Now;

        var uploadsDir = Path.Combine("wwwroot", "uploads");
        if (!Directory.Exists(uploadsDir))
            Directory.CreateDirectory(uploadsDir);

        // Handle Profile Photo upload
        if (dto.ProfilePhoto != null)
        {
            var uniqueFileName = Guid.NewGuid() + Path.GetExtension(dto.ProfilePhoto.FileName);
            var filePath = Path.Combine(uploadsDir, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await dto.ProfilePhoto.CopyToAsync(stream);
            }

            investor.Profilephotopath = "/uploads/" + uniqueFileName;
        }

        // Handle Logo upload
        if (dto.Logo != null)
        {
            var uniqueFileName = Guid.NewGuid() + Path.GetExtension(dto.Logo.FileName);
            var filePath = Path.Combine(uploadsDir, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await dto.Logo.CopyToAsync(stream);
            }

            investor.Logopath = "/uploads/" + uniqueFileName;
        }

        // Update many-to-many relations

        // Clear old collections
        investor.Industries.Clear();
        investor.Technologies.Clear();
        investor.Innovationmethods.Clear();
        investor.Developmentstages.Clear();

        // Re-add new relations
        investor.Industries = await _context.Industries.Where(i => dto.IndustryIds.Contains(i.Id)).ToListAsync();
        investor.Technologies = await _context.Technologies.Where(t => dto.TechnologyIds.Contains(t.Id)).ToListAsync();
        investor.Innovationmethods = await _context.Innovationmethods.Where(m => dto.InnovationMethodIds.Contains(m.Id)).ToListAsync();
        investor.Developmentstages = await _context.Developmentstages.Where(d => dto.DevelopmentStageIds.Contains(d.Id)).ToListAsync();

        // Save changes
        await _context.SaveChangesAsync();

        return Ok("Investor profile updated successfully");
    }


    // Admin: Delete investor
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
