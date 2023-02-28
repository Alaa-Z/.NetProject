
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
    public class OfferController : Controller
    {
        private readonly ApplicationDbContext _context;
        // to get the root path of my application
        private readonly IWebHostEnvironment _env;


        public OfferController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;

        }

        // GET: Offer
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Offers.Include(o => o.Service);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Offer/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Offers == null)
            {
                return NotFound();
            }

            var offer = await _context.Offers
                .Include(o => o.Service)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (offer == null)
            {
                return NotFound();
            }

            return View(offer);
        }

        // GET: Offer/Create
        public IActionResult Create()
        {
            ViewData["ServiceId"] = new SelectList(_context.Services, "Id", "Details");
            return View();
        }


        // POST: Offer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Details,Price,ImageFile,AltText,ServiceId")] Offer offer)
        {
            if (ModelState.IsValid)
            {
                // if image file exist
                if (offer.ImageFile != null)
                {
                    //Save the uploaded image file to wwwroot / uploads directory
                    var imagePath = Path.Combine(_env.WebRootPath, "uploads", offer.ImageFile.FileName);
                    // upload to folder using FileStream class 
                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await offer.ImageFile.CopyToAsync(stream);
                    }
                    offer.ImagePath = "/uploads/" + offer.ImageFile.FileName;
                }
                _context.Add(offer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(offer);
        }

      
        // POST: Offer/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Details,Price,ImageFile,AltText,ServiceId")] Offer offer)
        {
            if (id != offer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingOffer = await _context.Offers.FindAsync(id);

                    if (existingOffer == null)
                    {
                        return NotFound();
                    }

                    if (offer.ImageFile != null)
                    {
                        //Save the uploaded image file to wwwroot / uploads directory

                        var UpdatedimagePath = Path.Combine(_env.WebRootPath, "uploads", offer.ImageFile.FileName);
                        using (var stream = new FileStream(UpdatedimagePath, FileMode.Create))
                        {
                            await offer.ImageFile.CopyToAsync(stream);
                        }
                        offer.ImagePath = "/uploads/" + offer.ImageFile.FileName;

                    }
                    else
                    {
                        // if there is no new image then keep the old path 
                        offer.ImagePath = existingOffer.ImagePath;
                    }

                    // Before attaching the updated entity detach the existing entity 
                    _context.Entry(existingOffer).State = EntityState.Detached;

                    _context.Update(offer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OfferExists(offer.Id))
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
            return View(offer);
        }

        // GET: Offer/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Offers == null)
            {
                return NotFound();
            }

            var offer = await _context.Offers.FindAsync(id);
            if (offer == null)
            {
                return NotFound();
            }
            ViewData["ServiceId"] = new SelectList(_context.Services, "Id", "Details", offer.ServiceId);
            return View(offer);
        }

        // GET: Offer/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Offers == null)
            {
                return NotFound();
            }

            var offer = await _context.Offers
                .Include(o => o.Service)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (offer == null)
            {
                return NotFound();
            }

            return View(offer);
        }

        // POST: Offer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Offers == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Offers'  is null.");
            }
            var offer = await _context.Offers.FindAsync(id);
            if (offer != null)
            {
                _context.Offers.Remove(offer);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OfferExists(int id)
        {
          return (_context.Offers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
