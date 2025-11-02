using Domain.DTOS.Customer;
using Infrastructure.ApiResponse;
using Infrastructure.Interface.Customer;

namespace Infrastructure.Service.Customer;

public class CustomerGetService : ICustomerGetService
{
    public async Task<Response<List<CustomerGetDto>>> GetAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<Response<CustomerGetDto>> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }
}