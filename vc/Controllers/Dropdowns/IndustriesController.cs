using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vc.Models;
using vc.DTOs.DropdownDtos;

namespace vc.Controllers.Dropdowns
{
    [Route("api/[controller]")]
    [ApiController]
    public class IndustriesController : ControllerBase
    {
        private readonly VcdbContext _context;

        public IndustriesController(VcdbContext context)
        {
            _context = context;
        }

        // GET: api/Industries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IndustryDto>>> GetIndustries()
        {
            return await _context.Industries
                .Select(i => new IndustryDto { Id = i.Id, Name = i.Name })
                .ToListAsync();
        }


        // GET: api/Industries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IndustryDto>> GetIndustry(int id)
        {
            var industry = await _context.Industries.FindAsync(id);
            if (industry == null) return NotFound();

            return new IndustryDto { Id = industry.Id, Name = industry.Name };
        }


        // PUT: api/Industries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIndustry(int id, IndustryDto dto)
        {
            if (id != dto.Id) return BadRequest();

            var industry = await _context.Industries.FindAsync(id);
            if (industry == null) return NotFound();

            industry.Name = dto.Name;
            await _context.SaveChangesAsync();

            return NoContent();
        }


        // POST: api/Industries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<IndustryDto>> PostIndustry(IndustryDto dto)
        {
            var industry = new Industry { Name = dto.Name };
            _context.Industries.Add(industry);
            await _context.SaveChangesAsync();

            dto.Id = industry.Id;
            return CreatedAtAction(nameof(GetIndustry), new { id = dto.Id }, dto);
        }


        // DELETE: api/Industries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIndustry(int id)
        {
            var industry = await _context.Industries.FindAsync(id);
            if (industry == null) return NotFound();

            _context.Industries.Remove(industry);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        private bool IndustryExists(int id)
        {
            return _context.Industries.Any(e => e.Id == id);
        }
    }
}
