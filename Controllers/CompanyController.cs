using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhoneBook.Data;
using PhoneBook.Models;

namespace PhoneBook.Controllers
{
    public class CompanyController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CompanyController(ApplicationDbContext dbContext)
        {
            _context = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index() 
        { 
            var companies = await _context.Companies.ToListAsync();

            return View(companies);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,Address")] Company company) 
        {
            if (ModelState.IsValid)
            {
                await _context.Companies.AddAsync(company);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(company);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id) 
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Companies.FindAsync(id);

            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Address")] Company company)
        {
            if (id != company.Id)
            {
                return NotFound();
            }
            
            if (ModelState.IsValid)
            {
                _context.Update(company);

                await _context.SaveChangesAsync();

                return RedirectToAction("Index"); //RedirectToAction(nameof(Index));
            }

            return View(company);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Companies.
                AsNoTracking().
                FirstOrDefaultAsync(x => x.Id == id);

            if (company == null) 
            {
                return NotFound();
            }

            return View(company);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id) 
        {
            var company = await _context.Companies.FindAsync(id);

            if (company == null)
            {
                return RedirectToAction(nameof(Index));
            }

            _context.Companies.Remove(company);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");

        }
    }
}
