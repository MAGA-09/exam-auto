using Domain.DTOS.Rental;
using Infrastructure.ApiResponse;

namespace Infrastructure.Interface.Rental;

public interface IRentalCrudService
{
    Task<Response<string>> CreateAsync(RentalCreateDto dto);
    Task<Response<string>> UpdateAsync(int id, RentalUpdateDto dto);
    Task<Response<string>> DeleteAsync(int id);
}