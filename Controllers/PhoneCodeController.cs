using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhoneBook.Data;
using PhoneBook.Models;

namespace PhoneBook.Controllers
{
    public class PhoneCodeController : Controller
    {
        private readonly ApplicationDbContext _context;
        public PhoneCodeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var phoneCodes = await _context.PhoneCodes.ToListAsync();

            return View(phoneCodes);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] PhoneCode phoneCode)
        {
            if (ModelState.IsValid)
            {
                _context.Add(phoneCode);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(phoneCode);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phoneCode = await _context.PhoneCodes.FindAsync(id);

            if (phoneCode == null)
            {
                return NotFound();
            }

            return View(phoneCode);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] PhoneCode phoneCode)
        {
            if (id != phoneCode.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(phoneCode);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(phoneCode);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phoneCode = await _context.PhoneCodes.
                AsNoTracking().
                FirstOrDefaultAsync(x => x.Id == id);

            if (phoneCode == null)
            {
                return NotFound();
            }

            return View(phoneCode);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var phoneCode = await _context.PhoneCodes.FindAsync(id);

            if (phoneCode == null)
            {
                return RedirectToAction(nameof(Index));
            }

            _context.PhoneCodes.Remove(phoneCode);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
