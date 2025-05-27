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
    public class CountriesController : ControllerBase
    {
        private readonly VcdbContext _context;

        public CountriesController(VcdbContext context)
        {
            _context = context;
        }

        // GET: api/Countries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CountryDto>>> GetCountries()
        {
            return await _context.Countries
                .Select(c => new CountryDto
                {
                    Id = c.Id,
                    Name = c.Name
                }).ToListAsync();
        }

        // GET: api/Countries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CountryDto>> GetCountry(int id)
        {
            var country = await _context.Countries.FindAsync(id);
            if (country == null) return NotFound();

            return new CountryDto
            {
                Id = country.Id,
                Name = country.Name
            };
        }

        // PUT: api/Countries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCountry(int id, CountryDto dto)
        {
            if (id != dto.Id) return BadRequest();

            var country = await _context.Countries.FindAsync(id);
            if (country == null) return NotFound();

            country.Name = dto.Name;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Countries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CountryDto>> PostCountry(CountryDto dto)
        {
            var country = new Country { Name = dto.Name };
            _context.Countries.Add(country);
            await _context.SaveChangesAsync();

            dto.Id = country.Id;
            return CreatedAtAction(nameof(GetCountry), new { id = country.Id }, dto);
        }

        // DELETE: api/Countries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            var country = await _context.Countries.FindAsync(id);
            if (country == null) return NotFound();

            _context.Countries.Remove(country);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CountryExists(int id)
        {
            return _context.Countries.Any(e => e.Id == id);
        }
    }
}
