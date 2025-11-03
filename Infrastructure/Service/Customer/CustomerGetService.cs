using Domain.DTOS.Customer;
using Infrastructure.ApiResponse;
using Infrastructure.Data;
using Infrastructure.Enum;
using Infrastructure.Interface.Customer;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Service.Customer;

public class CustomerGetService(DataContext _context) : ICustomerGetService
{
    public async Task<Response<List<CustomerGetDto>>> GetAsync()
    {
        try
        {
            var customers = await _context.Customers
                .Select(c => new CustomerGetDto
                {
                    Id = c.Id,
                    FullName = c.FullName,
                    Phone = c.Phone,
                    Email = c.Email
                })
                .ToListAsync();

            return Response<List<CustomerGetDto>>.Ok(customers);
        }
        catch (Exception ex)
        {
            return Response<List<CustomerGetDto>>.Fail($"Ошибка при получении списка покупателей: {ex.Message}", ErrorType.Internal);
        }
    }

    public async Task<Response<CustomerGetDto>> GetByIdAsync(int id)
    {
        try
        {
            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.Id == id);

            if (customer == null)
                return Response<CustomerGetDto>.Fail("Покупатель не найден.", ErrorType.NotFound);

            var dto = new CustomerGetDto
            {
                Id = customer.Id,
                FullName = customer.FullName,
                Phone = customer.Phone,
                Email = customer.Email
            };

            return Response<CustomerGetDto>.Ok(dto);
        }
        catch (Exception ex)
        {
            return Response<CustomerGetDto>.Fail($"Ошибка при получении покупателя: {ex.Message}", ErrorType.Internal);
        }
    }
}
