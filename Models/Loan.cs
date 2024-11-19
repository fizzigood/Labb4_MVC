using System.ComponentModel.DataAnnotations;

namespace Labb4_MVC.Models
{
    public class Loan
    {
        [Key]
        public int LoanId { get; set; }

        [Required]
        public DateTime LoanDate { get; set; }

        public DateTime? ReturnDate { get; set; }

        [Required]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        [Required]
        public int BookId { get; set; }
        public Book Book { get; set; }
    }

}
