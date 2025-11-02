using Domain.DTOS.Customer;
using Infrastructure.ApiResponse;

namespace Infrastructure.Interface.Customer;

public interface ICustomerCrudService
{
    Task<Response<string>> CreateAsync(CustomerCreateDto dto);
    Task<Response<string>> UpdateAsync(int id, CustomerUpdateDto dto);
    Task<Response<string>> DeleteAsync(int id);
}