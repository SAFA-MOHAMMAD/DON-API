using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DON.Context;
using DON.Models;

namespace DON.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorProfilesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public InstructorProfilesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/InstructorProfiles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InstructorProfile>>> GetInstructorProfiles()
        {
            return await _context.InstructorProfiles.ToListAsync();
        }

        // GET: api/InstructorProfiles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<InstructorProfile>> GetInstructorProfile(int id)
        {
            var instructorProfile = await _context.InstructorProfiles.FindAsync(id);

            if (instructorProfile == null)
            {
                return NotFound();
            }

            return instructorProfile;
        }

        // PUT: api/InstructorProfiles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInstructorProfile(int id, InstructorProfile instructorProfile)
        {
            if (id != instructorProfile.Id)
            {
                return BadRequest();
            }

            _context.Entry(instructorProfile).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InstructorProfileExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/InstructorProfiles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<InstructorProfile>> PostInstructorProfile(InstructorProfile instructorProfile)
        {
            _context.InstructorProfiles.Add(instructorProfile);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInstructorProfile", new { id = instructorProfile.Id }, instructorProfile);
        }

        // DELETE: api/InstructorProfiles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInstructorProfile(int id)
        {
            var instructorProfile = await _context.InstructorProfiles.FindAsync(id);
            if (instructorProfile == null)
            {
                return NotFound();
            }

            _context.InstructorProfiles.Remove(instructorProfile);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool InstructorProfileExists(int id)
        {
            return _context.InstructorProfiles.Any(e => e.Id == id);
        }
    }
}
