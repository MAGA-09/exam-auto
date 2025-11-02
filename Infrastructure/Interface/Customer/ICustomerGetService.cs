using Domain.DTOS.Customer;
using Infrastructure.ApiResponse;

namespace Infrastructure.Interface.Customer;

public interface ICustomerGetService
{
    Task<Response<List<CustomerGetDto>>> GetAsync();
    Task<Response<CustomerGetDto>> GetByIdAsync(int id);
}