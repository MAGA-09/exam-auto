using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Car
{
    [Key]
    public int Id { get; set; }
    [Required, MaxLength(100)]
    public string Model { get; set; }
    [Required, MaxLength(100)]
    public string Manufacturer { get; set; }
    public int Year { get; set; }
    public decimal PricePerDay { get; set; }

    public ICollection<Rental> Rentals = new List<Rental>();
}