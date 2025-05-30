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
    public class SourceinfoesController : ControllerBase
    {
        private readonly VcdbContext _context;

        public SourceinfoesController(VcdbContext context)
        {
            _context = context;
        }

        // GET: api/Sourceinfoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SourceInfoDto>>> GetSourceinfos()
        {
            return await _context.Sourceinfos
                .Select(s => new SourceInfoDto { Id = s.Id, Name = s.Name })
                .ToListAsync();
        }

        // GET: api/Sourceinfoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SourceInfoDto>> GetSourceinfo(int id)
        {
            var s = await _context.Sourceinfos.FindAsync(id);
            if (s == null) return NotFound();

            return new SourceInfoDto { Id = s.Id, Name = s.Name };
        }

        // PUT: api/Sourceinfoes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSourceinfo(int id, SourceInfoDto dto)
        {
            if (id != dto.Id) return BadRequest();

            var entity = await _context.Sourceinfos.FindAsync(id);
            if (entity == null) return NotFound();

            entity.Name = dto.Name;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Sourceinfoes
        [HttpPost]
        public async Task<ActionResult<SourceInfoDto>> PostSourceinfo(SourceInfoDto dto)
        {
            var entity = new Sourceinfo { Name = dto.Name };
            _context.Sourceinfos.Add(entity);
            await _context.SaveChangesAsync();

            dto.Id = entity.Id;
            return CreatedAtAction(nameof(GetSourceinfo), new { id = dto.Id }, dto);
        }

        // DELETE: api/Sourceinfoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSourceinfo(int id)
        {
            var entity = await _context.Sourceinfos.FindAsync(id);
            if (entity == null) return NotFound();

            _context.Sourceinfos.Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SourceinfoExists(int id)
        {
            return _context.Sourceinfos.Any(e => e.Id == id);
        }
    }
}
