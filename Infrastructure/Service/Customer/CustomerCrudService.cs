using Domain.DTOS.Customer;
using Infrastructure.ApiResponse;
using Infrastructure.Data;
using Infrastructure.Enum;
using Infrastructure.Interface.Customer;
using Infrastructure.Validator.Customer;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Service.Customer;

public class CustomerCrudService(DataContext _context) : ICustomerCrudService
{
    public async Task<Response<string>> CreateAsync(CustomerCreateDto dto)
    {
        try
        {
            var validation = CreateValidator.ValidateCreate(dto);
            if (validation != null) return validation;
            
            bool exists = await _context.Customers.AnyAsync(c => c.Email.ToLower() == dto.Email.ToLower());
            if (exists)
                return Response<string>.Fail("Пользователь с таким email уже существует.", ErrorType.Conflict);

            var customer = new Domain.Entities.Customer
            {
                FullName = dto.FullName.Trim(),
                Phone = dto.Phone.Trim(),
                Email = dto.Email.Trim()
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return Response<string>.Ok("Покупатель успешно создан.");
        }
        catch (Exception ex)
        {
            return Response<string>.Fail($"Ошибка при создании покупателя: {ex.Message}", ErrorType.Internal);
        }
    }

    public async Task<Response<string>> UpdateAsync(int id, CustomerUpdateDto dto)
    {
        try
        {
            var validation = UpdateValidator.ValidateUpdate(dto);
            if (validation != null) return validation;
            
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == id);
            if (customer == null)
                return Response<string>.Fail("Покупатель не найден.", ErrorType.NotFound);
            
            bool exists = await _context.Customers.AnyAsync(c => c.Email.ToLower() == dto.Email.ToLower());
            if (exists)
                return Response<string>.Fail("Пользователь с таким email уже существует.", ErrorType.Conflict);
            
            customer.FullName = dto.FullName.Trim();
            customer.Phone = dto.Phone.Trim();
            customer.Email = dto.Email!.Trim();

            await _context.SaveChangesAsync();
            return Response<string>.Ok("Данные покупателя успешно обновлены.");
        }
        catch (Exception ex)
        {
            return Response<string>.Fail($"Ошибка при обновлении покупателя: {ex.Message}", ErrorType.Internal);
        }
    }

    public async Task<Response<string>> DeleteAsync(int id)
    {
        try
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == id);
            if (customer == null)
                return Response<string>.Fail("Покупатель не найден.", ErrorType.NotFound);

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return Response<string>.Ok("Покупатель успешно удалён.");
        }
        catch (Exception ex)
        {
            return Response<string>.Fail($"Ошибка при удалении покупателя: {ex.Message}", ErrorType.Internal);
        }
    }
}
