using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.DTOS.Car;
using Infrastructure.ApiResponse;
using Infrastructure.Data;
using Infrastructure.Enum;
using Infrastructure.Interface.Car;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Service.Car;

public class CarGetService(DataContext _context) : ICarGetService
{
    public async Task<Response<List<CarGetDto>>> GetAsync()
    {
        try
        {
            var cars = await _context.Cars
                .Select(c => new CarGetDto
                {
                    Id = c.Id,
                    Model = c.Model,
                    Manufacturer = c.Manufacturer,
                    Year = c.Year,
                    PricePerDay = c.PricePerDay
                }).ToListAsync();

            return Response<List<CarGetDto>>.Ok(cars);
        }
        catch (Exception ex)
        {
            return Response<List<CarGetDto>>.Fail($"Ошибка при получении автомобилей: {ex.Message}", ErrorType.Internal);
        }
    }

    public async Task<Response<CarGetDto>> GetByIdAsync(int id)
    {
        try
        {
            if (id <= 0)
                return Response<CarGetDto>.Fail("Некорректный идентификатор автомобиля.", ErrorType.Validation);

            var car = await _context.Cars
                .FirstOrDefaultAsync(c => c.Id == id);

            if (car == null)
                return Response<CarGetDto>.Fail("Автомобиль не найден.", ErrorType.NotFound);

            var dto = new CarGetDto
            {
                Id = car.Id,
                Model = car.Model,
                Manufacturer = car.Manufacturer,
                Year = car.Year,
                PricePerDay = car.PricePerDay
            };

            return Response<CarGetDto>.Ok(dto);
        }
        catch (Exception ex)
        {
            return Response<CarGetDto>.Fail($"Ошибка при получении автомобиля: {ex.Message}", ErrorType.Internal);
        }
    }
}
