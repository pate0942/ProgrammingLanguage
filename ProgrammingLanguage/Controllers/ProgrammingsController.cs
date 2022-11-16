using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProgrammingLanguage.Data;
using ProgrammingLanguage.Models;

namespace ProgrammingLanguage.Controllers
{
    public class ProgrammingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProgrammingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Programmings
        public async Task<IActionResult> Index()
        {
              return View(await _context.Programming.ToListAsync());
        }

        // GET : Programmings/ShowSearchForm
        public async Task<IActionResult> ShowSearchForm()
        {
            return View();
        }

        // POST : Programmings/ShowSearchResult
        public async Task<IActionResult> ShowSearchResults(String SearchPhrase)
        {
            return View("Index",await _context.Programming.Where(j => j.Question.Contains(SearchPhrase)).ToListAsync());
        }


        // GET: Programmings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Programming == null)
            {
                return NotFound();
            }

            var programming = await _context.Programming
                .FirstOrDefaultAsync(m => m.Id == id);
            if (programming == null)
            {
                return NotFound();
            }

            return View(programming);
        }

        // GET: Programmings/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Programmings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Question,Answer")] Programming programming)
        {
            if (ModelState.IsValid)
            {
                _context.Add(programming);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(programming);
        }

        // GET: Programmings/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Programming == null)
            {
                return NotFound();
            }

            var programming = await _context.Programming.FindAsync(id);
            if (programming == null)
            {
                return NotFound();
            }
            return View(programming);
        }

        // POST: Programmings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Question,Answer")] Programming programming)
        {
            if (id != programming.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(programming);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProgrammingExists(programming.Id))
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
            return View(programming);
        }

        // GET: Programmings/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Programming == null)
            {
                return NotFound();
            }

            var programming = await _context.Programming
                .FirstOrDefaultAsync(m => m.Id == id);
            if (programming == null)
            {
                return NotFound();
            }

            return View(programming);
        }

        // POST: Programmings/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Programming == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Programming'  is null.");
            }
            var programming = await _context.Programming.FindAsync(id);
            if (programming != null)
            {
                _context.Programming.Remove(programming);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProgrammingExists(int id)
        {
          return _context.Programming.Any(e => e.Id == id);
        }
    }
}
