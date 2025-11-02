using Domain.DTOS.Customer;
using Infrastructure.ApiResponse;
using Infrastructure.Interface.Customer;

namespace Infrastructure.Service.Customer;

public class CustomerCrudService : ICustomerCrudService
{
    public async Task<Response<string>> CreateAsync(CustomerCreateDto dto)
    {
        throw new NotImplementedException();
    }

    public async Task<Response<string>> UpdateAsync(int id, CustomerUpdateDto dto)
    {
        throw new NotImplementedException();
    }

    public async Task<Response<string>> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }
}