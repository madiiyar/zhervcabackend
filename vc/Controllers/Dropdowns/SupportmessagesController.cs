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
    public class SupportmessagesController : ControllerBase
    {
        private readonly VcdbContext _context;

        public SupportmessagesController(VcdbContext context)
        {
            _context = context;
        }

        // GET: api/Supportmessages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SupportMessageDto>>> GetSupportmessages()
        {
            return await _context.Supportmessages
                .Select(s => new SupportMessageDto
                {
                    Id = s.Id,
                    FullName = s.Fullname,
                    Email = s.Email,
                    Message = s.Message
                }).ToListAsync();
        }

        // GET: api/Supportmessages/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SupportMessageDto>> GetSupportmessage(int id)
        {
            var s = await _context.Supportmessages.FindAsync(id);
            if (s == null) return NotFound();

            return new SupportMessageDto
            {
                Id = s.Id,
                FullName = s.Fullname,
                Email = s.Email,
                Message = s.Message
            };
        }

        // PUT: api/Supportmessages/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSupportmessage(int id, SupportMessageDto dto)
        {
            if (id != dto.Id) return BadRequest();

            var entity = await _context.Supportmessages.FindAsync(id);
            if (entity == null) return NotFound();

            entity.Fullname = dto.FullName;
            entity.Email = dto.Email;
            entity.Message = dto.Message;

            await _context.SaveChangesAsync();
            return NoContent();
        }


        // POST: api/Supportmessages
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SupportMessageDto>> PostSupportmessage(SupportMessageDto dto)
        {
            var message = new Supportmessage
            {
                Fullname = dto.FullName,
                Email = dto.Email,
                Message = dto.Message
            };

            _context.Supportmessages.Add(message);
            await _context.SaveChangesAsync();

            dto.Id = message.Id;
            return CreatedAtAction(nameof(GetSupportmessage), new { id = dto.Id }, dto);
        }

        // DELETE: api/Supportmessages/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSupportmessage(int id)
        {
            var entity = await _context.Supportmessages.FindAsync(id);
            if (entity == null) return NotFound();

            _context.Supportmessages.Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SupportmessageExists(int id)
        {
            return _context.Supportmessages.Any(e => e.Id == id);
        }
    }
}
