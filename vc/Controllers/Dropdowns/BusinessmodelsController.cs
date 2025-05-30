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
    public class BusinessmodelsController : ControllerBase
    {
        private readonly VcdbContext _context;

        public BusinessmodelsController(VcdbContext context)
        {
            _context = context;
        }

        // GET: api/Businessmodels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BusinessModelDto>>> GetBusinessmodels()
        {
            return await _context.Businessmodels
                .Select(b => new BusinessModelDto
                {
                    Id = b.Id,
                    Name = b.Name
                })
                .ToListAsync();
        }

        // GET: api/Businessmodels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BusinessModelDto>> GetBusinessmodel(int id)
        {
            var businessmodel = await _context.Businessmodels.FindAsync(id);

            if (businessmodel == null)
                return NotFound();

            return new BusinessModelDto
            {
                Id = businessmodel.Id,
                Name = businessmodel.Name
            };
        }

        // PUT: api/Businessmodels/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBusinessmodel(int id, BusinessModelDto dto)
        {
            if (id != dto.Id)
                return BadRequest();

            var businessmodel = await _context.Businessmodels.FindAsync(id);
            if (businessmodel == null)
                return NotFound();

            businessmodel.Name = dto.Name;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // POST: api/Businessmodels
        [HttpPost]
        public async Task<ActionResult<BusinessModelDto>> PostBusinessmodel(BusinessModelDto dto)
        {
            var businessmodel = new Businessmodel
            {
                Name = dto.Name
            };

            _context.Businessmodels.Add(businessmodel);
            await _context.SaveChangesAsync();

            dto.Id = businessmodel.Id;

            return CreatedAtAction(nameof(GetBusinessmodel), new { id = dto.Id }, dto);
        }

        // DELETE: api/Businessmodels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBusinessmodel(int id)
        {
            var businessmodel = await _context.Businessmodels.FindAsync(id);
            if (businessmodel == null)
                return NotFound();

            _context.Businessmodels.Remove(businessmodel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BusinessmodelExists(int id)
        {
            return _context.Businessmodels.Any(e => e.Id == id);
        }
    }
}
