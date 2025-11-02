using Domain.DTOS.Car;
using Infrastructure.ApiResponse;

namespace Infrastructure.Interface.Car;

public interface ICarCrudService
{
    Task<Response<string>> CreateAsync(CarCreateDto dto);
    Task<Response<string>> UpdateAsync(int id, CarUpdateDto dto);
    Task<Response<string>> DeleteAsync(int id);
}