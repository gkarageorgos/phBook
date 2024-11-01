using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhoneBook.Data;
using PhoneBook.Models;

namespace PhoneBook.Controllers
{
    public class PhoneTypeController : Controller
    {
        private readonly ApplicationDbContext _context;
        public PhoneTypeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var phoneTypes = await _context.PhoneTypes.ToListAsync();

            return View(phoneTypes);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] PhoneType phoneType)
        {
            if (ModelState.IsValid)
            {
                await _context.PhoneTypes.AddAsync(phoneType);

                await _context.SaveChangesAsync();
                
                return RedirectToAction("Index");
            }

            return View(phoneType);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phoneType = await _context.PhoneTypes.FindAsync(id);


            if (phoneType == null)
            {
                return NotFound();
            }

            return View(phoneType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] PhoneType phoneType)
        {
            if (id != phoneType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(phoneType);

                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View(phoneType);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phoneType = await _context.PhoneTypes.
                AsNoTracking().
                FirstOrDefaultAsync(x => x.Id == id);

            if (phoneType == null)
            {
                return NotFound();
            }

            return View(phoneType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var phoneType = await _context.PhoneTypes.FindAsync(id);

            if (phoneType == null)
            {
                return RedirectToAction(nameof(Index));
            }

            _context.PhoneTypes.Remove(phoneType);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
