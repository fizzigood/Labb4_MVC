using Labb4_MVC.Models;
using Microsoft.EntityFrameworkCore;

namespace Labb4_MVCs.Data
{
    public class LibraryContext : DbContext
    {
        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options) { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Loan> Loans { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Loan>()
                .HasOne(l => l.Customer)
                .WithMany(c => c.Loans)
                .HasForeignKey(l => l.CustomerId);

            modelBuilder.Entity<Loan>()
                .HasOne(l => l.Book)
                .WithMany(b => b.Loans)
                .HasForeignKey(l => l.BookId);

            // Seed data
            modelBuilder.Entity<Customer>().HasData(
                new Customer { CustomerId = 1, Name = "John Doe", Email = "john.doe@example.com", PhoneNumber = "1234567890" },
                new Customer { CustomerId = 2, Name = "Jane Smith", Email = "jane.smith@example.com", PhoneNumber = "0987654321" }
            );

            modelBuilder.Entity<Book>().HasData(
                new Book { BookId = 1, Title = "C# Programming", Author = "Author A", IsReturned = true },
                new Book { BookId = 2, Title = "ASP.NET Core", Author = "Author B", IsReturned = false }
            );

            modelBuilder.Entity<Loan>().HasData(
                new Loan { LoanId = 1, LoanDate = DateTime.Now, CustomerId = 1, BookId = 1 },
                new Loan { LoanId = 2, LoanDate = DateTime.Now, CustomerId = 2, BookId = 2 }
            );
        }
    }
}
