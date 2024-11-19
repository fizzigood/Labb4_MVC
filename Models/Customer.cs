using System.ComponentModel.DataAnnotations;

namespace Labb4_MVC.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 16)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        public List<Loan> Loans { get; set; } = new();
    }
}
