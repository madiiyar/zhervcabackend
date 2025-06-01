using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using vc.DTOs;
using vc.Models;

namespace vc.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]  // Require logged-in user for all endpoints
    public class StartupsController : ControllerBase
    {
        private readonly VcdbContext _context;

        public StartupsController(VcdbContext context)
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


        // ✅ GET: Summary list
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StartupListDto>>> GetStartups(
                [FromQuery] List<int>? industryIds,
                [FromQuery] List<int>? technologyIds,
                [FromQuery] List<int>? businessModelIds,
                [FromQuery] List<int>? developmentStageIds,
                [FromQuery] List<int>? investmentStageIds,
                [FromQuery] List<int>? foundingYears,
                [FromQuery] int? minEmployees,
                [FromQuery] int? maxEmployees)
        {
            var query = _context.Startups
                .Include(s => s.Country)
                .Include(s => s.Developmentstage)
                .Include(s => s.Investmentstage)
                .Include(s => s.Businessmodels)
                .Include(s => s.Industries)
                .Include(s => s.Technologies)
                .AsQueryable();

            // Apply filters only if parameters are provided, otherwise skip

            if (industryIds != null && industryIds.Any())
            {
                foreach (var id in industryIds)
                {
                    query = query.Where(s => s.Industries.Any(i => i.Id == id));
                }
            }

            if (technologyIds != null && technologyIds.Any())
            {
                foreach (var id in technologyIds)
                {
                    query = query.Where(s => s.Technologies.Any(t => t.Id == id));
                }
            }

            if (businessModelIds != null && businessModelIds.Any())
            {
                query = query.Where(s => s.Businessmodels.Any(b => businessModelIds.Contains(b.Id)));
            }

            if (developmentStageIds != null && developmentStageIds.Any())
            {
                query = query.Where(s => developmentStageIds.Contains(s.Developmentstageid ?? 0));
            }

            if (investmentStageIds != null && investmentStageIds.Any())
            {
                query = query.Where(s => investmentStageIds.Contains(s.Investmentstageid ?? 0));
            }

            if (foundingYears != null && foundingYears.Any())
            {
                query = query.Where(s => foundingYears.Contains(s.Foundingyear ?? 0));
            }

            if (minEmployees.HasValue)
                query = query.Where(s => (s.Employeecount ?? 0) >= minEmployees.Value);

            if (maxEmployees.HasValue)
                query = query.Where(s => (s.Employeecount ?? 0) <= maxEmployees.Value);

            var results = await query.Select(s => new StartupListDto
            {
                Id = s.Id,
                PublicName = s.Publicname,
                OrganizationName = s.Organizationname,
                CountryName = s.Country.Name,
                DevelopmentStage = s.Developmentstage.Name,
                LogoPath = s.Logopath
            }).ToListAsync();

            return Ok(results);
        }


        // GET: api/startups - public summary list (no user required)
        [AllowAnonymous]
        [HttpGet("{publicName}")]
        public async Task<ActionResult<StartupDetailDto>> GetStartup(string publicName)
        {
            var startup = await _context.Startups
                .Include(s => s.Country)
                .Include(s => s.Developmentstage)
                .Include(s => s.Investmentstage)
                .Include(s => s.Industries)
                .Include(s => s.Technologies)
                .Include(s => s.Businessmodels)
                .Include(s => s.Salesmodels)
                .Include(s => s.Countries)
                .FirstOrDefaultAsync(s => s.Organizationname.ToLower() == publicName.ToLower());

            if (startup == null) return NotFound();

            return new StartupDetailDto
            {
                Id=startup.Id,
                PublicName = startup.Publicname,
                ContactFullName = startup.Contactfullname,
                PublicEmail = startup.Publicemail,
                PhoneNumber = startup.Phonenumber,
                Website = startup.Website,
                OrganizationName = startup.Organizationname,
                IdentificationNumber = startup.Identificationnumber,
                FoundingYear = startup.Foundingyear ?? 0,
                CountryName = startup.Country?.Name,
                EmployeeCount = startup.Employeecount ?? 0,
                Description = startup.Description,
                DevelopmentStage = startup.Developmentstage?.Name,
                InvestmentStage = startup.Investmentstage?.Name,
                HasSales = startup.Hassales ?? false,
                ActivelyLookingForInvestment = startup.Activelylookingforinvestment ?? false,
                TotalPreviousInvestment = startup.Totalpreviousinvestment ?? 0,
                InvestorList = startup.Investorlist,
                LogoPath = startup.Logopath,
                PresentationPath = startup.Presentationpath,
                Industries = startup.Industries.Select(i => i.Name).ToList(),
                Technologies = startup.Technologies.Select(t => t.Name).ToList(),
                BusinessModels = startup.Businessmodels.Select(b => b.Name).ToList(),
                SalesModels = startup.Salesmodels.Select(sm => sm.Name).ToList(),
                TargetCountries = startup.Countries.Select(c => c.Name).ToList()
            };
        }

        [Authorize(Roles = "Startup")]
        [HttpGet("me")]
        public async Task<ActionResult<StartupDetailDto>> GetMyStartup()
        {
            var userid = UserId;

            var startup = await _context.Startups
                .Include(s => s.Country)
                .Include(s => s.Developmentstage)
                .Include(s => s.Investmentstage)
                .Include(s => s.Industries)
                .Include(s => s.Technologies)
                .Include(s => s.Businessmodels)
                .Include(s => s.Salesmodels)
                .Include(s => s.Countries)
                .FirstOrDefaultAsync(s => s.Userid == userid);

            if (startup == null) return NotFound("Startup profile not found");

            return new StartupDetailDto
            {
                PublicName = startup.Publicname,
                ContactFullName = startup.Contactfullname,
                PublicEmail = startup.Publicemail,
                PhoneNumber = startup.Phonenumber,
                Website = startup.Website,
                OrganizationName = startup.Organizationname,
                IdentificationNumber = startup.Identificationnumber,
                FoundingYear = startup.Foundingyear ?? 0,
                CountryName = startup.Country?.Name,
                EmployeeCount = startup.Employeecount ?? 0,
                Description = startup.Description,
                DevelopmentStage = startup.Developmentstage?.Name,
                InvestmentStage = startup.Investmentstage?.Name,
                HasSales = startup.Hassales ?? false,
                ActivelyLookingForInvestment = startup.Activelylookingforinvestment ?? false,
                TotalPreviousInvestment = startup.Totalpreviousinvestment ?? 0,
                InvestorList = startup.Investorlist,
                LogoPath = startup.Logopath,
                PresentationPath = startup.Presentationpath,
                Industries = startup.Industries.Select(i => i.Name).ToList(),
                Technologies = startup.Technologies.Select(t => t.Name).ToList(),
                BusinessModels = startup.Businessmodels.Select(b => b.Name).ToList(),
                SalesModels = startup.Salesmodels.Select(sm => sm.Name).ToList(),
                TargetCountries = startup.Countries.Select(c => c.Name).ToList()
            };
        }

        // POST: api/startups - create new startup profile, user must be logged in
        [Authorize(Roles = "Startup")]
        [HttpPost]
        public async Task<IActionResult> CreateStartup([FromForm] StartupAnketaDto dto)
        {
            var userId = UserId;
            if (userId == null) return Unauthorized();

            var startup = new Startup
            {
                Userid = userId.Value,
                Publicname = dto.PublicName,
                Contactfullname = dto.ContactFullName,
                Publicemail = dto.PublicEmail,
                Phonenumber = dto.PhoneNumber,
                Website = dto.Website,
                Organizationname = dto.OrganizationName,
                Identificationnumber = dto.IdentificationNumber,
                Foundingyear = dto.FoundingYear,
                Countryid = dto.CountryId,
                Employeecount = dto.EmployeeCount,
                Description = dto.Description,
                Developmentstageid = dto.DevelopmentStageId,
                Investmentstageid = dto.InvestmentStageId,
                Hassales = dto.HasSales,
                Activelylookingforinvestment = dto.ActivelyLookingForInvestment,
                Totalpreviousinvestment = dto.TotalPreviousInvestment,
                Investorlist = dto.InvestorList,
                Sourceinfoid = dto.SourceInfoId,
                Createdat = DateTime.Now,
                Updatedat = DateTime.Now
            };

            if (dto.Logo != null)
            {
                var fileName = Guid.NewGuid() + System.IO.Path.GetExtension(dto.Logo.FileName);
                var path = System.IO.Path.Combine("wwwroot/uploads", fileName);
                using var stream = new System.IO.FileStream(path, System.IO.FileMode.Create);
                await dto.Logo.CopyToAsync(stream);
                startup.Logopath = "/uploads/" + fileName;
            }

            if (dto.Presentation != null)
            {
                var fileName = Guid.NewGuid() + System.IO.Path.GetExtension(dto.Presentation.FileName);
                var path = System.IO.Path.Combine("wwwroot/uploads", fileName);
                using var stream = new System.IO.FileStream(path, System.IO.FileMode.Create);
                await dto.Presentation.CopyToAsync(stream);
                startup.Presentationpath = "/uploads/" + fileName;
            }

            _context.Startups.Add(startup);
            await _context.SaveChangesAsync();

            // Assign many-to-many relationships
            startup.Industries = await _context.Industries.Where(i => dto.IndustryIds.Contains(i.Id)).ToListAsync();
            startup.Technologies = await _context.Technologies.Where(t => dto.TechnologyIds.Contains(t.Id)).ToListAsync();
            startup.Businessmodels = await _context.Businessmodels.Where(b => dto.BusinessModelIds.Contains(b.Id)).ToListAsync();
            startup.Salesmodels = await _context.Salesmodels.Where(s => dto.SalesModelIds.Contains(s.Id)).ToListAsync();
            startup.Countries = await _context.Countries.Where(c => dto.TargetCountryIds.Contains(c.Id)).ToListAsync();

            await _context.SaveChangesAsync();

            // Fetch full entity with navigation for response
            var createdStartup = await _context.Startups
                .Include(s => s.Country)
                .Include(s => s.Developmentstage)
                .Include(s => s.Investmentstage)
                .Include(s => s.Industries)
                .Include(s => s.Technologies)
                .Include(s => s.Businessmodels)
                .Include(s => s.Salesmodels)
                .Include(s => s.Countries)
                .FirstOrDefaultAsync(s => s.Id == startup.Id);

            if (createdStartup == null) return NotFound();

            var dtoResult = new StartupDetailDto
            {
                PublicName = createdStartup.Publicname,
                ContactFullName = createdStartup.Contactfullname,
                PublicEmail = createdStartup.Publicemail,
                PhoneNumber = createdStartup.Phonenumber,
                Website = createdStartup.Website,
                OrganizationName = createdStartup.Organizationname,
                IdentificationNumber = createdStartup.Identificationnumber,
                FoundingYear = createdStartup.Foundingyear ?? 0,
                CountryName = createdStartup.Country?.Name,
                EmployeeCount = createdStartup.Employeecount ?? 0,
                Description = createdStartup.Description,
                DevelopmentStage = createdStartup.Developmentstage?.Name,
                InvestmentStage = createdStartup.Investmentstage?.Name,
                HasSales = createdStartup.Hassales ?? false,
                ActivelyLookingForInvestment = createdStartup.Activelylookingforinvestment ?? false,
                TotalPreviousInvestment = createdStartup.Totalpreviousinvestment ?? 0,
                InvestorList = createdStartup.Investorlist,
                LogoPath = createdStartup.Logopath,
                PresentationPath = createdStartup.Presentationpath,
                Industries = createdStartup.Industries.Select(i => i.Name).ToList(),
                Technologies = createdStartup.Technologies.Select(t => t.Name).ToList(),
                BusinessModels = createdStartup.Businessmodels.Select(b => b.Name).ToList(),
                SalesModels = createdStartup.Salesmodels.Select(sm => sm.Name).ToList(),
                TargetCountries = createdStartup.Countries.Select(c => c.Name).ToList()
            };

            return CreatedAtAction(nameof(GetStartup), new { publicName = dtoResult.PublicName }, dtoResult);
        }


        // PUT: api/startups - update startup profile of logged-in user
        [Authorize(Roles = "Startup")]
        [HttpPut]
        public async Task<IActionResult> UpdateStartup([FromForm] StartupAnketaDto dto)
        {
            var userId = UserId;
            if (userId == null) return Unauthorized();

            var startup = await _context.Startups
                .Include(s => s.Industries)
                .Include(s => s.Technologies)
                .Include(s => s.Businessmodels)
                .Include(s => s.Salesmodels)
                .Include(s => s.Countries)
                .FirstOrDefaultAsync(s => s.Userid == userId.Value);

            if (startup == null) return NotFound();

            startup.Publicname = dto.PublicName;
            startup.Contactfullname = dto.ContactFullName;
            startup.Publicemail = dto.PublicEmail;
            startup.Phonenumber = dto.PhoneNumber;
            startup.Website = dto.Website;
            startup.Organizationname = dto.OrganizationName;
            startup.Identificationnumber = dto.IdentificationNumber;
            startup.Foundingyear = dto.FoundingYear;
            startup.Countryid = dto.CountryId;
            startup.Employeecount = dto.EmployeeCount;
            startup.Description = dto.Description;
            startup.Developmentstageid = dto.DevelopmentStageId;
            startup.Investmentstageid = dto.InvestmentStageId;
            startup.Hassales = dto.HasSales;
            startup.Activelylookingforinvestment = dto.ActivelyLookingForInvestment;
            startup.Totalpreviousinvestment = dto.TotalPreviousInvestment;
            startup.Investorlist = dto.InvestorList;
            startup.Sourceinfoid = dto.SourceInfoId;
            startup.Updatedat = DateTime.Now;

            if (dto.Logo != null)
            {
                var fileName = Guid.NewGuid() + System.IO.Path.GetExtension(dto.Logo.FileName);
                var path = System.IO.Path.Combine("wwwroot/uploads", fileName);
                using var stream = new System.IO.FileStream(path, System.IO.FileMode.Create);
                await dto.Logo.CopyToAsync(stream);
                startup.Logopath = "/uploads/" + fileName;
            }

            if (dto.Presentation != null)
            {
                var fileName = Guid.NewGuid() + System.IO.Path.GetExtension(dto.Presentation.FileName);
                var path = System.IO.Path.Combine("wwwroot/uploads", fileName);
                using var stream = new System.IO.FileStream(path, System.IO.FileMode.Create);
                await dto.Presentation.CopyToAsync(stream);
                startup.Presentationpath = "/uploads/" + fileName;
            }

            // Clear and reassign many-to-many
            startup.Industries.Clear();
            startup.Technologies.Clear();
            startup.Businessmodels.Clear();
            startup.Salesmodels.Clear();
            startup.Countries.Clear();

            startup.Industries = await _context.Industries.Where(i => dto.IndustryIds.Contains(i.Id)).ToListAsync();
            startup.Technologies = await _context.Technologies.Where(t => dto.TechnologyIds.Contains(t.Id)).ToListAsync();
            startup.Businessmodels = await _context.Businessmodels.Where(b => dto.BusinessModelIds.Contains(b.Id)).ToListAsync();
            startup.Salesmodels = await _context.Salesmodels.Where(s => dto.SalesModelIds.Contains(s.Id)).ToListAsync();
            startup.Countries = await _context.Countries.Where(c => dto.TargetCountryIds.Contains(c.Id)).ToListAsync();

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/startups/{id} - admin only (you can add [Authorize(Roles = "Admin")] later)
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStartup(int id)
        {
            var startup = await _context.Startups.FindAsync(id);
            if (startup == null) return NotFound();

            _context.Startups.Remove(startup);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
