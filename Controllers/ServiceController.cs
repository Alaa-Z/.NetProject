using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Models;

namespace Project.Controllers
{
    public class ServiceController : Controller
    {
        private readonly ApplicationDbContext _context;
        // to get the root path of my application
        private readonly IWebHostEnvironment _env; 


        public ServiceController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: Service
        public async Task<IActionResult> Index()
        {
              return _context.Services != null ? 
                          View(await _context.Services.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Services'  is null.");
        }

        // GET: Service/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Services == null)
            {
                return NotFound();
            }

            var service = await _context.Services
                .FirstOrDefaultAsync(m => m.Id == id);
            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        // GET: Service/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Service/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Details,ImageFile,AltText")] Service service)
        {
            if (ModelState.IsValid)
            {
                // if image file exist
                if (service.ImageFile != null)
                {
                    //Save the uploaded image file to wwwroot / uploads directory
                    var imagePath = Path.Combine(_env.WebRootPath, "uploads", service.ImageFile.FileName);
                    // upload to folder using FileStream class 
                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await service.ImageFile.CopyToAsync(stream);
                    }
                    service.ImagePath = "/uploads/" + service.ImageFile.FileName;
                }
                _context.Add(service);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(service);
        }
       

        //POST: Service/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Details,ImageFile,AltText")] Service service)
        {
            if (id != service.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingService = await _context.Services.FindAsync(id);

                    if (existingService == null)
                    {
                        return NotFound();
                    }

                    if (service.ImageFile != null)
                    {
                        //Save the uploaded image file to wwwroot / uploads directory

                        var UpdatedimagePath = Path.Combine(_env.WebRootPath, "uploads", service.ImageFile.FileName);
                        using (var stream = new FileStream(UpdatedimagePath, FileMode.Create))
                        {
                            await service.ImageFile.CopyToAsync(stream);
                        }
                        service.ImagePath = "/uploads/" + service.ImageFile.FileName;

                    }
                    else
                    {
                        // if there is no new image then keep the old path 
                        service.ImagePath = existingService.ImagePath;
                    }

                    // Before attaching the updated entity detach the existing entity 
                    _context.Entry(existingService).State = EntityState.Detached;

                    _context.Update(service);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceExists(service.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(service);
        }



        // GET: Service/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Services == null)
            {
                return NotFound();
            }

            var service = await _context.Services.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }
            return View(service);
        }

        // GET: Service/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Services == null)
            {
                return NotFound();
            }

            var service = await _context.Services
                .FirstOrDefaultAsync(m => m.Id == id);
            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        // POST: Service/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Services == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Services'  is null.");
            }
            var service = await _context.Services.FindAsync(id);
            if (service != null)
            {
                _context.Services.Remove(service);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServiceExists(int id)
        {
          return (_context.Services?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
