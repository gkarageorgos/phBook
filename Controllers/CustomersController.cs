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
    public class CustomersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Customers
        public async Task<IActionResult> Index()
        {
            var customers = _context.Customers.
                Include(c => c.Company).
                Include(c => c.Emails).
                Include(c => c.Phones).
                AsNoTracking();
            return View(await customers.ToListAsync());
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
               .Include(c => c.Company)
               .Include(c => c.Emails)
               .Include(c => c.Phones)
               .AsNoTracking()
               .FirstOrDefaultAsync(m => m.Id == id);

            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", selectedValue: null)
                .Prepend(new SelectListItem { Text = "Καμία", Value = "" });
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Surname,Notes,CompanyId")] Customer customer)
        {
            if (string.IsNullOrWhiteSpace(customer.FullName))
            {
                ModelState.AddModelError("", "Είναι απαραίτητο να συμπληρωθεί τουλάχιστον ένα από τα πεδία, Όνομα ή Επώνυμο ή Εταιρεία.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", selectedValue: null)
                 .Prepend(new SelectListItem { Text = "Καμία", Value = "" });
            return View(customer);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.
                Include(x => x.Company).
                Include(x => x.Emails).
                Include(x => x.Phones).
                FirstOrDefaultAsync(c => c.Id == id);

            if (customer == null)
            {
                return NotFound();
            }

            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", selectedValue: null)
                .Prepend(new SelectListItem { Text = "Καμία", Value = "" });
            ViewData["EmailTypeId"] = new SelectList(_context.EmailTypes, "Id", "Name");
            ViewData["PhoneCodeId"] = new SelectList(_context.PhoneCodes, "Id", "Name");
            ViewData["PhoneTypeId"] = new SelectList(_context.PhoneTypes, "Id", "Name");

            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Surname,Notes,CompanyId")] Customer customer)
        {
            if (id != customer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.Id))
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

            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", selectedValue: null)
                .Prepend(new SelectListItem { Text = "Καμία", Value = "" });
            return View(customer);
        }
        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .Include(c => c.Company)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        // GET: Customers/AddEmail/5
        public IActionResult AddEmail(int customerId)
        {
            if (customerId == 0)
            {
                return NotFound();
            }

            ViewData["EmailTypeId"] = new SelectList(_context.EmailTypes, "Id", "Name");

            return View(new Email { CustomerId = customerId });
        }

        // POST: Customers/AddEmail/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Route("Customers/{customerId}/Emails/Create")]
        public async Task<IActionResult> AddEmail(int customerId, [Bind("CustomerId,EmailAddress,EmailTypeId")] Email email)
        {
            if (customerId != email.CustomerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Add(email);

                await _context.SaveChangesAsync();

                return RedirectToAction("Edit", new { Id = email.CustomerId });
            }

            ViewData["EmailTypeId"] = new SelectList(_context.EmailTypes, "Id", "Name", email.EmailTypeId);

            return View(email);
        }

        public async Task<IActionResult> EditEmail(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var email = await _context.Emails.FindAsync(id);
            if (email == null)
            {
                return NotFound();
            }

            ViewData["EmailTypeId"] = new SelectList(_context.EmailTypes, "Id", "Name");

            return View(email);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditEmail(int id, [Bind("Id,EmailAddress,EmailTypeId,CustomerId")] Email email)
        {
            if (id != email.Id)
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {
                _context.Update(email);

                await _context.SaveChangesAsync();

                return RedirectToAction("Edit", new { Id = email.CustomerId });
            }

            ViewData["EmailTypeId"] = new SelectList(_context.EmailTypes, "Id", "Name");

            return View(email);
        }

        public async Task<IActionResult> DeleteEmail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var email = await _context.Emails
                .Include(e => e.EmailType)
                .Include(e => e.Customer)
                .FirstOrDefaultAsync(e =>  e.Id == id);

            if (email == null)
            {
                return NotFound();
            }

            return View(email);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteEmail(int id)
        {
            var email = await _context.Emails.FindAsync(id);

            if (email != null)
            {
                _context.Emails.Remove(email);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Edit", new { Id = email.CustomerId });
        }

        // GET: Customers/AddPhone/5
        //[Route("Customers/{customerId}/Phones/Create")]
        public IActionResult AddPhone(int customerId)
        {
            if (customerId == 0)
            {
                return NotFound();
            }

            ViewData["PhoneCodeId"] = new SelectList(_context.PhoneCodes, "Id", "Name");
            ViewData["PhoneTypeId"] = new SelectList(_context.PhoneTypes, "Id", "Name");

            return View(new Phone() { CustomerId = customerId});
        }

        // POST: Customers/AddPhone/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Route("Customers/{customerId}/Phones/Create")]
        public async Task<IActionResult> AddPhone(int customerId, [Bind("CustomerId,PhoneNumber,PhoneCodeId,PhoneTypeId")] Phone phone)
        {
            if (customerId != phone.CustomerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Add(phone);

                await _context.SaveChangesAsync();

                return RedirectToAction("Edit", new { Id = phone.CustomerId });
            }

            ViewData["PhoneCodeId"] = new SelectList(_context.PhoneCodes, "Id", "Name", phone.PhoneCodeId);
            ViewData["PhoneTypeId"] = new SelectList(_context.PhoneTypes, "Id", "Name", phone.PhoneTypeId);

            return View(phone);
        }

        public async Task<IActionResult> EditPhone(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phone = await _context.Phones.FindAsync(id);
            if (phone == null)
            {
                return NotFound();
            }

            ViewData["PhoneCodeId"] = new SelectList(_context.PhoneCodes, "Id", "Name");
            ViewData["PhoneTypeId"] = new SelectList(_context.PhoneTypes, "Id", "Name");

            return View(phone);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPhone(int id, [Bind("Id,PhoneNumber,PhoneCodeId,PhoneTypeId,CustomerId")] Phone phone)
        {
            if (id != phone.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(phone);

                await _context.SaveChangesAsync();

                return RedirectToAction("Edit", new { Id = phone.CustomerId });
            }

            ViewData["PhoneCodeId"] = new SelectList(_context.PhoneCodes, "Id", "Name");
            ViewData["PhoneTypeId"] = new SelectList(_context.PhoneTypes, "Id", "Name");

            return View(phone);
        }

        public async Task<IActionResult> DeletePhone(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phone = await _context.Phones
                .Include(e => e.PhoneCode)
                .Include(e => e.PhoneType)
                .Include(e => e.Customer)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (phone == null)
            {
                return NotFound();
            }

            return View(phone);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePhone(int id)
        {
            var phone = await _context.Phones.FindAsync(id);

            if (phone != null)
            {
                _context.Phones.Remove(phone);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Edit", new { Id = phone.CustomerId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _AddEmail(int customerId, [Bind("CustomerId,EmailAddress,EmailTypeId")] Email email)
        {
            if (customerId != email.CustomerId)
            {
                return NotFound();
            }

            //ModelState.AddModelError(string.Empty, "Test Error");

            if (ModelState.IsValid)
            {
                _context.Add(email);

                await _context.SaveChangesAsync();

                return RedirectToAction("Edit", new { Id = email.CustomerId });
            }

            ViewData["EmailTypeId"] = new SelectList(_context.EmailTypes, "Id", "Name", email.EmailTypeId);

            return View(email);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _AddPhone(int customerId, [Bind("CustomerId,PhoneNumber,PhoneCodeId,PhoneTypeId")] Phone phone)
        {
            if (customerId != phone.CustomerId)
            {
                return NotFound();
            }

            //ModelState.AddModelError(string.Empty, "Test Error");

            if (ModelState.IsValid)
            {
                _context.Add(phone);

                await _context.SaveChangesAsync();

                return RedirectToAction("Edit", new { Id = phone.CustomerId });
            }

            ViewData["PhoneCodeId"] = new SelectList(_context.PhoneCodes, "Id", "Name", phone.PhoneCodeId);
            ViewData["PhoneTypeId"] = new SelectList(_context.PhoneTypes, "Id", "Name", phone.PhoneTypeId);

            return View(phone);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _EditEmail(int id, [Bind("Id,EmailAddress,EmailTypeId,CustomerId")] Email email)
        {
            if (id != email.Id)
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {
                _context.Update(email);

                await _context.SaveChangesAsync();

                return RedirectToAction("Edit", new { Id = email.CustomerId });
            }

            ViewData["EmailTypeId"] = new SelectList(_context.EmailTypes, "Id", "Name");

            return View(email);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _EditPhone(int id, [Bind("Id,PhoneNumber,PhoneCodeId,PhoneTypeId,CustomerId")] Phone phone)
        {
            if (id != phone.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(phone);

                await _context.SaveChangesAsync();

                return RedirectToAction("Edit", new { Id = phone.CustomerId });
            }

            ViewData["PhoneCodeId"] = new SelectList(_context.PhoneCodes, "Id", "Name");
            ViewData["PhoneTypeId"] = new SelectList(_context.PhoneTypes, "Id", "Name");

            return View(phone);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _DeleteEmail(int id)
        {
            var email = await _context.Emails.FindAsync(id);

            if (email != null)
            {
                _context.Emails.Remove(email);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Edit", new { Id = email.CustomerId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _DeletePhone(int id)
        {
            var phone = await _context.Phones.FindAsync(id);

            if (phone != null)
            {
                _context.Phones.Remove(phone);
                await _context.SaveChangesAsync();

            }

            return RedirectToAction("Edit", new { Id = phone.CustomerId });
        }
    }
}
