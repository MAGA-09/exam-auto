using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Rental
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CarId { get; set; }
        public Car Car { get; set; }  // без new()

        [Required]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }  // без new()

        [Required]
        public int BranchId { get; set; }
        public Branche Branche { get; set; }  // без new()

        [Required]
        public DateTime StartDate { get; set; } = DateTime.Now;

        [Required]
        public DateTime EndDate { get; set; }

        public decimal TotalCost { get; set; }
    }
}