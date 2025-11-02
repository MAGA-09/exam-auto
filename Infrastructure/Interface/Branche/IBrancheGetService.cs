using Domain.DTOS.Branch;
using Infrastructure.ApiResponse;

namespace Infrastructure.Interface.Branche;

public interface IBrancheGetService
{
    Task<Response<List<BrancheGetDto>>> GetAsync();
    Task<Response<BrancheGetDto>> GetByIdAsync(int id);
}