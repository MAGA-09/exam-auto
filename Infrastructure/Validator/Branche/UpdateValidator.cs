using Domain.DTOS.Branch;
using Infrastructure.ApiResponse;
using Infrastructure.Enum;

namespace Infrastructure.Validator.Branche;

public class UpdateValidator
{
    public static Response<string>? ValidateUpdate(BrancheUpdateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            return Response<string>.Fail("Название филиала не может быть пустым.", ErrorType.Validation);

        if (dto.Name.Length < 2)
            return Response<string>.Fail("Название филиала должно содержать минимум 2 символа.", ErrorType.Validation);

        if (string.IsNullOrWhiteSpace(dto.Location))
            return Response<string>.Fail("Локация филиала не может быть пустой.", ErrorType.Validation);

        
        return null;
    }
}