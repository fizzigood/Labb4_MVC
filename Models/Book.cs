using System.ComponentModel.DataAnnotations;

namespace Labb4_MVC.Models
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }

        [Required]
        public string Title { get; set; }

        public string Author { get; set; }

        public bool IsReturned { get; set; } = false;

        public List<Loan> Loans { get; set; } = new();
    }
}
