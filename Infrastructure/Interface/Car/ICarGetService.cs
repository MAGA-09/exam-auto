using Domain.DTOS.Car;
using Infrastructure.ApiResponse;

namespace Infrastructure.Interface.Car;

public interface ICarGetService
{
    Task<Response<List<CarGetDto>>> GetAsync();
    Task<Response<CarGetDto>> GetByIdAsync(int id);
}