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
    public class TechnologiesController : ControllerBase
    {
        private readonly VcdbContext _context;

        public TechnologiesController(VcdbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TechnologyDto>>> GetTechnologies()
        {
            return await _context.Technologies
                .Select(t => new TechnologyDto { Id = t.Id, Name = t.Name })
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TechnologyDto>> GetTechnology(int id)
        {
            var t = await _context.Technologies.FindAsync(id);
            if (t == null) return NotFound();

            return new TechnologyDto { Id = t.Id, Name = t.Name };
        }


        [HttpPost]
        public async Task<ActionResult<TechnologyDto>> PostTechnology(TechnologyDto dto)
        {
            var tech = new Technology { Name = dto.Name };
            _context.Technologies.Add(tech);
            await _context.SaveChangesAsync();

            dto.Id = tech.Id;
            return CreatedAtAction(nameof(GetTechnology), new { id = tech.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTechnology(int id, TechnologyDto dto)
        {
            if (id != dto.Id) return BadRequest();

            var tech = await _context.Technologies.FindAsync(id);
            if (tech == null) return NotFound();

            tech.Name = dto.Name;
            await _context.SaveChangesAsync();

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTechnology(int id)
        {
            var tech = await _context.Technologies.FindAsync(id);
            if (tech == null) return NotFound();

            _context.Technologies.Remove(tech);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        private bool TechnologyExists(int id)
        {
            return _context.Technologies.Any(e => e.Id == id);
        }
    }
}
