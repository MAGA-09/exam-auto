using Domain.DTOS.Rental;
using Infrastructure.ApiResponse;

namespace Infrastructure.Interface.Rental;

public interface IRentalGetService
{
    Task<Response<List<RentalGetDto>>> GetAsync();
    Task<Response<RentalGetDto>> GetByIdAsync(int id);
}