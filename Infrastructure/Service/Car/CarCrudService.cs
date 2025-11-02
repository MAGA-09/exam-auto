using System;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.DTOS.Car;
using Infrastructure.ApiResponse;
using Infrastructure.Data;
using Infrastructure.Enum;
using Infrastructure.Interface.Car;
using Infrastructure.Validator.Car;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Service.Car;

public class CarCrudService(DataContext _context) : ICarCrudService
{
    public async Task<Response<string>> CreateAsync(CarCreateDto dto)
    {
        try
        {
            var validation = CreateValidator.ValidateCreate(dto);
            if (validation != null) return validation;
            
            var entity = new Domain.Entities.Car
            {
                Model = dto.Model.Trim(),
                Manufacturer = dto.Manufacturer.Trim(),
                Year = dto.Year,
                PricePerDay = dto.PricePerDay
            };

            _context.Cars.Add(entity);
            await _context.SaveChangesAsync();

            return Response<string>.Ok("Автомобиль успешно создан.");
        }
        catch (Exception ex)
        {
            return Response<string>.Fail($"Ошибка при создании автомобиля: {ex.Message}", ErrorType.Internal);
        }
    }

    public async Task<Response<string>> UpdateAsync(int id, CarUpdateDto dto)
    {
        try
        {
            var validation = UpdateValidator.ValidateUpdate(dto);
            if (validation != null) return validation;
            
            if (id <= 0)
                return Response<string>.Fail("Некорректный идентификатор автомобиля.", ErrorType.Validation);

            var car = await _context.Cars.FirstOrDefaultAsync(c => c.Id == id);
            if (car == null)
                return Response<string>.Fail("Автомобиль не найден.", ErrorType.NotFound);

            car.Model = dto.Model.Trim();
            car.Manufacturer = dto.Manufacturer.Trim();
            car.Year = dto.Year;
            car.PricePerDay = dto.PricePerDay;

            await _context.SaveChangesAsync();

            return Response<string>.Ok("Автомобиль успешно обновлён.");
        }
        catch (Exception ex)
        {
            return Response<string>.Fail($"Ошибка при обновлении автомобиля: {ex.Message}", ErrorType.Internal);
        }
    }

    public async Task<Response<string>> DeleteAsync(int id)
    {
        try
        {
            if (id <= 0)
                return Response<string>.Fail("Некорректный идентификатор автомобиля.", ErrorType.Validation);

            var car = await _context.Cars.FirstOrDefaultAsync(c => c.Id == id);
            if (car == null)
                return Response<string>.Fail("Автомобиль не найден.", ErrorType.NotFound);

            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();

            return Response<string>.Ok("Автомобиль успешно удалён.");
        }
        catch (Exception ex)
        {
            return Response<string>.Fail($"Ошибка при удалении автомобиля: {ex.Message}", ErrorType.Internal);
        }
    }
}