using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IronParadise.Data;
using IronParadise.Models;

namespace IronParadise.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FitnessInfoesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FitnessInfoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/FitnessInfoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FitnessInfo>>> GetFitnessInfo()
        {
          if (_context.FitnessInfo == null)
          {
              return NotFound();
          }
            return await _context.FitnessInfo.ToListAsync();
        }

        // GET: api/FitnessInfoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FitnessInfo>> GetFitnessInfo(Guid id)
        {
          if (_context.FitnessInfo == null)
          {
              return NotFound();
          }
            var fitnessInfo = await _context.FitnessInfo.FindAsync(id);

            if (fitnessInfo == null)
            {
                return NotFound();
            }

            return fitnessInfo;
        }

        // PUT: api/FitnessInfoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFitnessInfo(Guid id, FitnessInfo fitnessInfo)
        {
            if (id != fitnessInfo.Id)
            {
                return BadRequest();
            }

            _context.Entry(fitnessInfo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FitnessInfoExists(id))
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

        // POST: api/FitnessInfoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<FitnessInfo>> PostFitnessInfo(FitnessInfo fitnessInfo)
        {
          if (_context.FitnessInfo == null)
          {
              return Problem("Entity set 'ApplicationDbContext.FitnessInfo'  is null.");
          }
            _context.FitnessInfo.Add(fitnessInfo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFitnessInfo", new { id = fitnessInfo.Id }, fitnessInfo);
        }

        // DELETE: api/FitnessInfoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFitnessInfo(Guid id)
        {
            if (_context.FitnessInfo == null)
            {
                return NotFound();
            }
            var fitnessInfo = await _context.FitnessInfo.FindAsync(id);
            if (fitnessInfo == null)
            {
                return NotFound();
            }

            _context.FitnessInfo.Remove(fitnessInfo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FitnessInfoExists(Guid id)
        {
            return (_context.FitnessInfo?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
