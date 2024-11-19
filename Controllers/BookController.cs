using Labb4_MVCs.Data;
using Labb4_MVC.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace Labb4_MVC.Controllers
{
    public class BookController : Controller
    {
        private readonly LibraryContext _context;

        public BookController(LibraryContext context)
        {
            _context = context;
        }

        // GET: /Book
        public async Task<IActionResult> Index()
        {
            var books = await _context.Books.ToListAsync();
            return View(books);
        }

        // GET: /Book/Details/{id}
        public async Task<IActionResult> Details(int id)
        {
            var book = await _context.Books
                .Include(b => b.Loans)
                .ThenInclude(l => l.Customer)
                .FirstOrDefaultAsync(b => b.BookId == id);

            if (book == null)
                return NotFound();

            return View(book);
        }

        // GET: /Book/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Book/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book book)
        {
            if (ModelState.IsValid)
            {
                _context.Books.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: /Book/Edit/{id}
        public async Task<IActionResult> Edit(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
                return NotFound();

            return View(book);
        }

        // POST: /Book/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Book book)
        {
            if (id != book.BookId)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.BookId))
                        return NotFound();

                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: /Book/Delete/{id}
        public async Task<IActionResult> Delete(int id)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b => b.BookId == id);
            if (book == null)
                return NotFound();

            return View(book);
        }

        // POST: /Book/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // Hjälpmetod för att kontrollera om en bok finns
        private bool BookExists(int id)
        {
            return _context.Books.Any(b => b.BookId == id);
        }
    }
}
