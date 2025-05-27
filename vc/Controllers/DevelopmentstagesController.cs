using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vc.Models;
using vc.DTOs;

namespace vc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevelopmentstagesController : ControllerBase
    {
        private readonly VcdbContext _context;

        public DevelopmentstagesController(VcdbContext context)
        {
            _context = context;
        }

        // GET: api/Developmentstages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DevelopmentStageDto>>> GetDevelopmentstages()
        {
            return await _context.Developmentstages
                .Select(d => new DevelopmentStageDto
                {
                    Id = d.Id,
                    Name = d.Name
                })
                .ToListAsync();
        }

        // GET: api/Developmentstages/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DevelopmentStageDto>> GetDevelopmentstage(int id)
        {
            var d = await _context.Developmentstages.FindAsync(id);
            if (d == null) return NotFound();

            return new DevelopmentStageDto
            {
                Id = d.Id,
                Name = d.Name
            };
        }

        // PUT: api/Developmentstages/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDevelopmentstage(int id, DevelopmentStageDto dto)
        {
            if (id != dto.Id) return BadRequest();

            var d = await _context.Developmentstages.FindAsync(id);
            if (d == null) return NotFound();

            d.Name = dto.Name;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Developmentstages
        [HttpPost]
        public async Task<ActionResult<DevelopmentStageDto>> PostDevelopmentstage(DevelopmentStageDto dto)
        {
            var d = new Developmentstage { Name = dto.Name };

            _context.Developmentstages.Add(d);
            await _context.SaveChangesAsync();

            dto.Id = d.Id;
            return CreatedAtAction(nameof(GetDevelopmentstage), new { id = d.Id }, dto);
        }

        // DELETE: api/Developmentstages/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDevelopmentstage(int id)
        {
            var d = await _context.Developmentstages.FindAsync(id);
            if (d == null) return NotFound();

            _context.Developmentstages.Remove(d);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DevelopmentstageExists(int id)
        {
            return _context.Developmentstages.Any(e => e.Id == id);
        }
    }
}
