using Labb4_MVCs.Data;
using Labb4_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Labb4_MVC.Controllers
{
    public class LoanController : Controller
    {
        private readonly LibraryContext _context;

        public LoanController(LibraryContext context)
        {
            _context = context;
        }

        // GET: Loan
        public async Task<IActionResult> Index()
        {
            var loans = _context.Loans
                .Include(l => l.Customer)
                .Include(l => l.Book);
            return View(await loans.ToListAsync());
        }

        // GET: Loan/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Loans == null)
            {
                return NotFound();
            }

            var loan = await _context.Loans
                .Include(l => l.Customer)
                .Include(l => l.Book)
                .FirstOrDefaultAsync(m => m.LoanId == id);
            if (loan == null)
            {
                return NotFound();
            }

            return View(loan);
        }

        // GET: Loan/Create
        public IActionResult Create()
        {
            ViewData["BookId"] = new SelectList(_context.Books, "BookId", "Title");
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "Name");
            return View();
        }

        // POST: Loan/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LoanId,BookId,CustomerId,LoanDate,ReturnDate,IsReturned")] Loan loan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(loan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookId"] = new SelectList(_context.Books, "BookId", "Title", loan.BookId);
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "Name", loan.CustomerId);
            return View(loan);
        }

        // GET: Loan/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Loans == null)
            {
                return NotFound();
            }

            var loan = await _context.Loans.FindAsync(id);
            if (loan == null)
            {
                return NotFound();
            }
            ViewData["BookId"] = new SelectList(_context.Books, "BookId", "Title", loan.BookId);
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "Name", loan.CustomerId);
            return View(loan);
        }

        // POST: Loan/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LoanId,BookId,CustomerId,LoanDate,ReturnDate,IsReturned")] Loan loan)
        {
            if (id != loan.LoanId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(loan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoanExists(loan.LoanId))
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
            ViewData["BookId"] = new SelectList(_context.Books, "BookId", "Title", loan.BookId);
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "Name", loan.CustomerId);
            return View(loan);
        }

        // GET: Loan/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Loans == null)
            {
                return NotFound();
            }

            var loan = await _context.Loans
                .Include(l => l.Customer)
                .Include(l => l.Book)
                .FirstOrDefaultAsync(m => m.LoanId == id);
            if (loan == null)
            {
                return NotFound();
            }

            return View(loan);
        }

        // POST: Loan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Loans == null)
            {
                return Problem("Entity set 'LibraryContext.Loans'  is null.");
            }
            var loan = await _context.Loans.FindAsync(id);
            if (loan != null)
            {
                _context.Loans.Remove(loan);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LoanExists(int id)
        {
            return (_context.Loans?.Any(e => e.LoanId == id)).GetValueOrDefault();
        }
    }
}
