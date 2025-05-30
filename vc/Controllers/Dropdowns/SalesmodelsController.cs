using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vc.DTOs.DropdownDtos;
using vc.Models;

namespace vc.Controllers.Dropdowns
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesmodelsController : ControllerBase
    {
        private readonly VcdbContext _context;

        public SalesmodelsController(VcdbContext context)
        {
            _context = context;
        }

        // GET: api/Salesmodels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SalesModelDto>>> GetSalesmodels()
        {
            return await _context.Salesmodels
                .Select(s => new SalesModelDto { Id = s.Id, Name = s.Name })
                .ToListAsync();
        }

        // GET: api/Salesmodels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SalesModelDto>> GetSalesmodel(int id)
        {
            var s = await _context.Salesmodels.FindAsync(id);
            if (s == null) return NotFound();

            return new SalesModelDto { Id = s.Id, Name = s.Name };
        }

        // PUT: api/Salesmodels/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSalesmodel(int id, SalesModelDto dto)
        {
            if (id != dto.Id) return BadRequest();

            var s = await _context.Salesmodels.FindAsync(id);
            if (s == null) return NotFound();

            s.Name = dto.Name;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Salesmodels
        [HttpPost]
        public async Task<ActionResult<SalesModelDto>> PostSalesmodel(SalesModelDto dto)
        {
            var s = new Salesmodel { Name = dto.Name };
            _context.Salesmodels.Add(s);
            await _context.SaveChangesAsync();

            dto.Id = s.Id;
            return CreatedAtAction(nameof(GetSalesmodel), new { id = s.Id }, dto);
        }

        // DELETE: api/Salesmodels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSalesmodel(int id)
        {
            var s = await _context.Salesmodels.FindAsync(id);
            if (s == null) return NotFound();

            _context.Salesmodels.Remove(s);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SalesmodelExists(int id)
        {
            return _context.Salesmodels.Any(e => e.Id == id);
        }
    }
}
