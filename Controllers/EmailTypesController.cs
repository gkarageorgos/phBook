using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PhoneBook.Data;
using PhoneBook.Models;

namespace PhoneBook.Controllers
{
    public class EmailTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmailTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: EmailTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.EmailTypes.ToListAsync());
        }

        // GET: EmailTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emailType = await _context.EmailTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (emailType == null)
            {
                return NotFound();
            }

            return View(emailType);
        }

        // GET: EmailTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EmailTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] EmailType emailType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(emailType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(emailType);
        }

        // GET: EmailTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emailType = await _context.EmailTypes.FindAsync(id);
            if (emailType == null)
            {
                return NotFound();
            }
            return View(emailType);
        }

        // POST: EmailTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id","Name")] EmailType emailType)
        {
            if (id != emailType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(emailType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmailTypeExists(emailType.Id))
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
            return View(emailType);
        }

        // GET: EmailTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emailType = await _context.EmailTypes.
                AsNoTracking().
                FirstOrDefaultAsync(m => m.Id == id);

            if (emailType == null)
            {
                return NotFound();
            }

            return View(emailType);
        }

        // POST: EmailTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var emailType = await _context.EmailTypes.FindAsync(id);

            if (emailType == null) 
            {
                return RedirectToAction(nameof(Index));
            }

            _context.EmailTypes.Remove(emailType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmailTypeExists(int id)
        {
            return _context.EmailTypes.Any(e => e.Id == id);
        }
    }
}
