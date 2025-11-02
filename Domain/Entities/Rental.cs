using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Rental
{
    [Key]
    public int Id { get; set; }
    [Required]
    public int CarId { get; set; }
    [Required]
    public int CustomerId { get; set; }
    [Required]
    public int BranchId { get; set; }
    [Required] 
    public DateTime StartDate { get; set; } = DateTime.Now;
    [Required]
    public DateTime EndDate { get; set; }
    public decimal TotalCost { get; set; }
    
    public Car Car = new();
    public Branche Branche = new();
    public Customer Customer = new();
}