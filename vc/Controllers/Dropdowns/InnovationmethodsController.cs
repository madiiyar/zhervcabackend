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
    public class InnovationmethodsController : ControllerBase
    {
        private readonly VcdbContext _context;

        public InnovationmethodsController(VcdbContext context)
        {
            _context = context;
        }

        // GET: api/Innovationmethods
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InnovationMethodDto>>> GetInnovationMethod()
        {
            return await _context.Innovationmethods
                .Select(i => new InnovationMethodDto { Id = i.Id, Name = i.Name! })
                .ToListAsync();
        }

        // GET: api/Innovationmethods/5
        [HttpGet("{id}")]
        public async Task<ActionResult<InnovationMethodDto>> GetInnovationMethod(int id)
        {
            var item = await _context.Innovationmethods.FindAsync(id);
            if (item == null) return NotFound();

            return new InnovationMethodDto { Id = item.Id, Name = item.Name! };
        }

        // PUT: api/Innovationmethods/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, InnovationMethodDto dto)
        {
            if (id != dto.Id) return BadRequest();

            var entity = await _context.Innovationmethods.FindAsync(id);
            if (entity == null) return NotFound();

            entity.Name = dto.Name;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // POST: api/Innovationmethods
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<InnovationMethodDto>> Post(InnovationMethodDto dto)
        {
            var entity = new Innovationmethod { Name = dto.Name };
            _context.Innovationmethods.Add(entity);
            await _context.SaveChangesAsync();

            dto.Id = entity.Id;
            return CreatedAtAction(nameof(GetInnovationMethod), new { id = dto.Id }, dto);
        }

        // DELETE: api/Innovationmethods/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _context.Innovationmethods.FindAsync(id);
            if (entity == null) return NotFound();

            _context.Innovationmethods.Remove(entity);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool InnovationmethodExists(int id)
        {
            return _context.Innovationmethods.Any(e => e.Id == id);
        }
    }
}
