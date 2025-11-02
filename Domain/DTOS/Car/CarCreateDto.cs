namespace Domain.DTOS.Car;

public class CarCreateDto
{
    public string Model { get; set; }
    public string Manufacturer { get; set; }
    public int Year { get; set; }
    public decimal PricePerDay { get; set; }
}