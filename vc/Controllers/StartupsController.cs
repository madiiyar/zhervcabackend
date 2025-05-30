using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vc.DTOs;
using vc.Models;

namespace vc.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StartupsController : ControllerBase
    {
        private readonly VcdbContext _context;

        public StartupsController(VcdbContext context)
        {
            _context = context;
        }

        //GET: Summary list
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StartupListDto>>> GetStartups()
        {
            return await _context.Startups
                .Include(s => s.Country)
                .Include(s => s.Developmentstage)
                .Select(s => new StartupListDto
                {
                    PublicName = s.Publicname,
                    CountryName = s.Country.Name,
                    DevelopmentStage = s.Developmentstage.Name,
                    LogoPath = s.Logopath
                })
                .ToListAsync();
        }

        // ✅ GET: Full profile by organization name
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

        // ✅ POST: Create
        [HttpPost]
        public async Task<IActionResult> CreateStartup([FromForm] StartupAnketaDto dto)
        {
            var userId = 3;

            var startup = new Startup
            {
                Userid = userId,
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
                var fileName = Guid.NewGuid() + Path.GetExtension(dto.Logo.FileName);
                var path = Path.Combine("wwwroot/uploads", fileName);
                using var stream = new FileStream(path, FileMode.Create);
                await dto.Logo.CopyToAsync(stream);
                startup.Logopath = "/uploads/" + fileName;
            }

            if (dto.Presentation != null)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(dto.Presentation.FileName);
                var path = Path.Combine("wwwroot/uploads", fileName);
                using var stream = new FileStream(path, FileMode.Create);
                await dto.Presentation.CopyToAsync(stream);
                startup.Presentationpath = "/uploads/" + fileName;
            }

            _context.Startups.Add(startup);
            await _context.SaveChangesAsync();

            // Join tables
            startup.Industries = await _context.Industries.Where(i => dto.IndustryIds.Contains(i.Id)).ToListAsync();
            startup.Technologies = await _context.Technologies.Where(t => dto.TechnologyIds.Contains(t.Id)).ToListAsync();
            startup.Businessmodels = await _context.Businessmodels.Where(b => dto.BusinessModelIds.Contains(b.Id)).ToListAsync();
            startup.Salesmodels = await _context.Salesmodels.Where(s => dto.SalesModelIds.Contains(s.Id)).ToListAsync();
            startup.Countries = await _context.Countries.Where(c => dto.TargetCountryIds.Contains(c.Id)).ToListAsync();

            await _context.SaveChangesAsync();
            return Ok("Startup profile created");
        }

        // ✅ PUT: Update
        [HttpPut]
        public async Task<IActionResult> UpdateStartup([FromForm] StartupAnketaDto dto)
        {
            var userId = 4;

            var startup = await _context.Startups
                .Include(s => s.Industries)
                .Include(s => s.Technologies)
                .Include(s => s.Businessmodels)
                .Include(s => s.Salesmodels)
                .Include(s => s.Countries)
                .FirstOrDefaultAsync(s => s.Userid == userId);

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
                var fileName = Guid.NewGuid() + Path.GetExtension(dto.Logo.FileName);
                var path = Path.Combine("wwwroot/uploads", fileName);
                using var stream = new FileStream(path, FileMode.Create);
                await dto.Logo.CopyToAsync(stream);
                startup.Logopath = "/uploads/" + fileName;
            }

            if (dto.Presentation != null)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(dto.Presentation.FileName);
                var path = Path.Combine("wwwroot/uploads", fileName);
                using var stream = new FileStream(path, FileMode.Create);
                await dto.Presentation.CopyToAsync(stream);
                startup.Presentationpath = "/uploads/" + fileName;
            }

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
            return Ok("Startup profile updated");
        }

        // ✅ DELETE
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
