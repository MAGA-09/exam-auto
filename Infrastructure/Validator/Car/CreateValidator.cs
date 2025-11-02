using Domain.DTOS.Car;
using Infrastructure.ApiResponse;
using Infrastructure.Enum;

namespace Infrastructure.Validator.Car;

public class CreateValidator
{
    public static Response<string>? ValidateCreate(CarCreateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Model))
            return Response<string>.Fail("Модель автомобиля не может быть пустой.", ErrorType.Validation);

        if (string.IsNullOrWhiteSpace(dto.Manufacturer))
            return Response<string>.Fail("Производитель не может быть пустым.", ErrorType.Validation);

        if (dto.Year <= 0)
            return Response<string>.Fail("Некорректный год выпуска.", ErrorType.Validation);

        if (dto.PricePerDay <= 0)
            return Response<string>.Fail("Цена за день должна быть положительной.", ErrorType.Validation);

        return null;
    }
}