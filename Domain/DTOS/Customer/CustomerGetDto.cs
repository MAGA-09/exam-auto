namespace Domain.DTOS.Customer;

public class CustomerGetDto
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Phone { get; set; }
    public string? Email { get; set; }
}