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
    public class MemberController : Controller
    {
        private readonly ApplicationDbContext _context;
        // to get the root path of my application
        private readonly IWebHostEnvironment _env;

        public MemberController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;

        }

        // GET: Member
        public async Task<IActionResult> Index(string searchString, string sortOrder)
        {
            // to add sorting 
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            var members = from m in _context.Members
                           select m;

            // To search for a member 
            ViewData["CurrentFilter"] = searchString;
            var myContext = _context.Members;

            // if we entered a search string 
            if (!String.IsNullOrEmpty(searchString))
            {
                var query = myContext.Where(c => c.Name.Contains(searchString));

                return View(await query.AsNoTracking().ToListAsync());
            }

            // to handle the sorting 
            switch (sortOrder)
            {
                case "name_desc":
                    members = members.OrderByDescending(s => s.Name);
                    break;

                default:
                    members = members.OrderBy(s => s.Name);
                    break;
            }

            return View(await members.AsNoTracking().ToListAsync());

        }

        // GET: Member/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Members == null)
            {
                return NotFound();
            }

            var member = await _context.Members
                .FirstOrDefaultAsync(m => m.Id == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // GET: Member/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Member/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,JobTitle,Email,ImageFile,AltText")] Member member)
        {
            if (ModelState.IsValid)
            {
                
                if (member.ImageFile != null)
                {
                //Save the uploaded image file to wwwroot / uploads directory
                var imagePath = Path.Combine(_env.WebRootPath, "uploads", member.ImageFile.FileName);
                // upload to folder using FileStream class 
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await member.ImageFile.CopyToAsync(stream);
                }
                    member.ImagePath = "/uploads/" + member.ImageFile.FileName;
                }
                _context.Add(member);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        // GET: Member/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Members == null)
            {
                return NotFound();
            }

            var member = await _context.Members.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }
            return View(member);
        }

        // POST: Member/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,JobTitle,Email,ImageFile,AltText")] Member member)
        {
           

            if (id != member.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingMemeber = await _context.Members.FindAsync(id);

                    if (existingMemeber == null)
                    {
                        return NotFound();
                    }

                    if (member.ImageFile != null)
                    {
                        //Save the uploaded image file to wwwroot / uploads directory

                        var UpdatedimagePath = Path.Combine(_env.WebRootPath, "uploads", member.ImageFile.FileName);
                        using (var stream = new FileStream(UpdatedimagePath, FileMode.Create))
                        {
                            await member.ImageFile.CopyToAsync(stream);
                        }
                        member.ImagePath = "/uploads/" + member.ImageFile.FileName;

                    }
                    else
                    {
                        // if there is no new image then keep the old path 
                        member.ImagePath = existingMemeber.ImagePath;
                    }

                    // Before attaching the updated entity detach the existing entity 
                    _context.Entry(existingMemeber).State = EntityState.Detached;

                    _context.Update(member);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberExists(member.Id))
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
            return View(member);
        }

        // GET: Member/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Members == null)
            {
                return NotFound();
            }

            var member = await _context.Members
                .FirstOrDefaultAsync(m => m.Id == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // POST: Member/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Members == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Member'  is null.");
            }
            var member = await _context.Members.FindAsync(id);
            if (member != null)
            {
                _context.Members.Remove(member);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MemberExists(int id)
        {
          return (_context.Members?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
