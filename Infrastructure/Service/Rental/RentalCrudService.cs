using System;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.DTOS.Rental;
using Infrastructure.ApiResponse;
using Infrastructure.Data;
using Infrastructure.Enum;
using Infrastructure.Interface.Rental;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Service.Rental;

public class RentalCrudService(DataContext _context) : IRentalCrudService
{
    public async Task<Response<string>> CreateAsync(RentalCreateDto dto)
    {
        try
        {
            var car = await _context.Cars.FirstOrDefaultAsync(c => c.Id == dto.CarId);
            if (car == null)
                return Response<string>.Fail("Автомобиль не найден.", ErrorType.NotFound);

            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == dto.CustomerId);
            if (customer == null)
                return Response<string>.Fail("Клиент не найден.", ErrorType.NotFound);

            var branch = await _context.Branches.FirstOrDefaultAsync(b => b.Id == dto.BranchId);
            if (branch == null)
                return Response<string>.Fail("Филиал не найден.", ErrorType.NotFound);

            bool busy = await _context.Rentals.AnyAsync(r =>
                r.CarId == dto.CarId &&
                ((dto.StartDate >= r.StartDate && dto.StartDate <= r.EndDate) ||
                 (dto.EndDate >= r.StartDate && dto.EndDate <= r.EndDate)));

            if (busy)
                return Response<string>.Fail("Автомобиль уже арендован на выбранные даты.", ErrorType.Conflict);

            if (dto.EndDate <= dto.StartDate)
                return Response<string>.Fail("Дата окончания должна быть позже даты начала.", ErrorType.Validation);

            var days = (dto.EndDate - dto.StartDate).Days;
            if (days <= 0)
                days = 1;

            decimal totalCost = car.PricePerDay * days;

            var rental = new Domain.Entities.Rental
            {
                CarId = dto.CarId,
                CustomerId = dto.CustomerId,
                BranchId = dto.BranchId,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                TotalCost = totalCost
            };

            _context.Rentals.Add(rental);
            await _context.SaveChangesAsync();

            return Response<string>.Ok($"Аренда успешно создана. Общая стоимость: {totalCost}$");
        }
        catch (Exception ex)
        {
            return Response<string>.Fail($"Ошибка при создании аренды: {ex.Message}", ErrorType.Internal);
        }
    }

    public async Task<Response<string>> UpdateAsync(int id, RentalUpdateDto dto)
    {
        try
        {
            var rental = await _context.Rentals.FirstOrDefaultAsync(r => r.Id == id);
            if (rental == null)
                return Response<string>.Fail("Аренда не найдена.", ErrorType.NotFound);

            if (dto.StartDate != default)
                rental.StartDate = dto.StartDate;

            if (dto.EndDate != default)
                rental.EndDate = dto.EndDate;

            var car = await _context.Cars.FirstOrDefaultAsync(c => c.Id == rental.CarId);
            if (car != null)
            {
                int days = (rental.EndDate - rental.StartDate).Days;
                if (days <= 0) days = 1;
                rental.TotalCost = car.PricePerDay * days;
            }

            await _context.SaveChangesAsync();
            return Response<string>.Ok("Аренда успешно обновлена.");
        }
        catch (Exception ex)
        {
            return Response<string>.Fail($"Ошибка при обновлении аренды: {ex.Message}", ErrorType.Internal);
        }
    }

    public async Task<Response<string>> DeleteAsync(int id)
    {
        try
        {
            var rental = await _context.Rentals.FirstOrDefaultAsync(r => r.Id == id);
            if (rental == null)
                return Response<string>.Fail("Аренда не найдена.", ErrorType.NotFound);

            _context.Rentals.Remove(rental);
            await _context.SaveChangesAsync();

            return Response<string>.Ok("Аренда успешно удалена.");
        }
        catch (Exception ex)
        {
            return Response<string>.Fail($"Ошибка при удалении аренды: {ex.Message}", ErrorType.Internal);
        }
    }
}