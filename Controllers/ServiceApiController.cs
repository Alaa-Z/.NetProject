using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Models;

namespace Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ServiceApiController(ApplicationDbContext context)
        {
            _context = context;
        }

      
        // GET: api/ServiceApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Service>>> GetServices()
        {
            var services = await _context.Services.ToListAsync();
            services.ForEach(service =>
            {
                if (!string.IsNullOrEmpty(service.ImagePath))
                {
                    service.ImagePath = $"https://localhost:7014{service.ImagePath}";
                }

              
            });
            return services;
        }


        // GET: api/ServiceApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Service>> GetService(int id)
        {
            if (_context.Services == null)
            {
                return NotFound();
            }
            var service = await _context.Services.FindAsync(id);

            if (service == null)
            {
                return NotFound();
            }

            return service;
        }

        // PUT: api/ServiceApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutService(int id, Service service)
        {
            if (id != service.Id)
            {
                return BadRequest();
            }

            _context.Entry(service).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServiceExists(id))
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

        // POST: api/ServiceApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Service>> PostService(Service service)
        {
            if (_context.Services == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Services'  is null.");
            }
            _context.Services.Add(service);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetService", new { id = service.Id }, service);
        }

        // DELETE: api/ServiceApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteService(int id)
        {
            if (_context.Services == null)
            {
                return NotFound();
            }
            var service = await _context.Services.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }

            _context.Services.Remove(service);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ServiceExists(int id)
        {
            return (_context.Services?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        // Get offers inside a service 
        //   api/ServiceApi/{serviceId}/Offers
        [HttpGet("{serviceId}/offers")]
        public IActionResult GetOffersForService(int serviceId)
        {
            var service = _context.Services.Include(s => s.Offer).SingleOrDefault(s => s.Id == serviceId);

            if (service == null)
            {
                return NotFound();
            }

            var offers = service.Offer;

            return Ok(offers);
        }
    }
}
