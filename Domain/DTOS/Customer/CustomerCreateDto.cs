namespace Domain.DTOS.Customer;

public class CustomerCreateDto
{
    public string FullName { get; set; }
    public string Phone { get; set; }
    public string? Email { get; set; }
}