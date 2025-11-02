using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Branche
{
    [Key]
    public int Id { get; set; }
    [Required, MaxLength(100)]
    public string Name { get; set; }
    [Required, MaxLength(200)]
    public string Location { get; set; }

    public ICollection<Car> Cars = new List<Car>();
}