using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Customer
{
    [Key]
    public int Id { get; set; }
    [Required, MaxLength(150)]
    public string FullName { get; set; }
    [Required, MaxLength(20)]
    public string Phone { get; set; }
    [MaxLength(150)]
    public string? Email { get; set; }

    public ICollection<Rental> Rentals = new List<Rental>();
}