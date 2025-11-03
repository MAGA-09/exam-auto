using Domain.DTOS.Customer;
using Infrastructure.ApiResponse;
using Infrastructure.Enum;

namespace Infrastructure.Validator.Customer;

public class UpdateValidator
{
    public static Response<string>? ValidateUpdate(CustomerUpdateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.FullName))
            return Response<string>.Fail("Имя не может быть пустым.", ErrorType.Validation);

        if (string.IsNullOrWhiteSpace(dto.Phone))
            return Response<string>.Fail("Фамилия не может быть пустой.", ErrorType.Validation);

        if (string.IsNullOrWhiteSpace(dto.Email))
            return Response<string>.Fail("Email обязателен.", ErrorType.Validation);

        return null;
    }
}