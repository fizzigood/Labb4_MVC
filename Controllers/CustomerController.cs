using Labb4_MVC.Models;
using Labb4_MVCs.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Labb4_MVC.Controllers
{
    public class CustomerController : Controller
    {
        private readonly LibraryContext _context;

        public CustomerController(LibraryContext context)
        {
            _context = context;
        }

        // GET: /Customer
        public async Task<IActionResult> Index()
        {
            var customers = await _context.Customers.ToListAsync();
            return View(customers);
        }

        // GET: /Customer/Details/{id}
        public async Task<IActionResult> Details(int id)
        {
            var customer = await _context.Customers
                .Include(c => c.Loans)
                .ThenInclude(l => l.Book)
                .FirstOrDefaultAsync(c => c.CustomerId == id);

            if (customer == null)
                return NotFound();

            return View(customer);
        }

        // GET: /Customer/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Customer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: /Customer/Edit/{id}
        public async Task<IActionResult> Edit(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
                return NotFound();

            return View(customer);
        }

        // POST: /Customer/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Customer customer)
        {
            if (id != customer.CustomerId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Customers.Any(c => c.CustomerId == id))
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: /Customer/Delete/{id}
        public async Task<IActionResult> Delete(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
                return NotFound();

            return View(customer);
        }

        // POST: /Customer/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: /Customer/Loans/{id}
        public async Task<IActionResult> Loans(int id)
        {
            var customer = await _context.Customers
                .Include(c => c.Loans)
                .ThenInclude(l => l.Book)
                .FirstOrDefaultAsync(c => c.CustomerId == id);

            if (customer == null)
                return NotFound();

            return View(customer);
        }

        // GET: /Customer/LoanBook/{customerId}
        public async Task<IActionResult> LoanBook(int customerId)
        {
            var customer = await _context.Customers.FindAsync(customerId);
            if (customer == null)
                return NotFound();

            ViewBag.Books = new SelectList(_context.Books.Where(b => !b.Loans.Any(l => l.ReturnDate == null)), "BookId", "Title");
            return View(new Loan { CustomerId = customerId });
        }

        // POST: /Customer/LoanBook
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoanBook(Loan loan)
        {
            if (ModelState.IsValid)
            {
                loan.LoanDate = DateTime.Now;
                _context.Loans.Add(loan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { id = loan.CustomerId });
            }
            ViewBag.Books = new SelectList(_context.Books.Where(b => !b.Loans.Any(l => l.ReturnDate == null)), "BookId", "Title");
            return View(loan);
        }
    }
}
