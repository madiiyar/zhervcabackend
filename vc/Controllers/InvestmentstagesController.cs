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
    public class InvestmentstagesController : ControllerBase
    {
        private readonly VcdbContext _context;

        public InvestmentstagesController(VcdbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InvestmentStageDto>>> GetInvestmentstages()
        {
            return await _context.Investmentstages
                .Select(x => new InvestmentStageDto { Id = x.Id, Name = x.Name })
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<InvestmentStageDto>> GetInvestmentstage(int id)
        {
            var entity = await _context.Investmentstages.FindAsync(id);
            if (entity == null) return NotFound();

            return new InvestmentStageDto { Id = entity.Id, Name = entity.Name };
        }

        [HttpPost]
        public async Task<ActionResult<InvestmentStageDto>> PostInvestmentstage(InvestmentStageDto dto)
        {
            var entity = new Investmentstage { Name = dto.Name };
            _context.Investmentstages.Add(entity);
            await _context.SaveChangesAsync();

            dto.Id = entity.Id;
            return CreatedAtAction(nameof(GetInvestmentstage), new { id = dto.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutInvestmentstage(int id, InvestmentStageDto dto)
        {
            if (id != dto.Id) return BadRequest();

            var entity = await _context.Investmentstages.FindAsync(id);
            if (entity == null) return NotFound();

            entity.Name = dto.Name;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInvestmentstage(int id)
        {
            var entity = await _context.Investmentstages.FindAsync(id);
            if (entity == null) return NotFound();

            _context.Investmentstages.Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool InvestmentstageExists(int id)
        {
            return _context.Investmentstages.Any(e => e.Id == id);
        }
    }
}
