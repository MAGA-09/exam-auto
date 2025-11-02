using Domain.DTOS.Branch;
using Infrastructure.ApiResponse;

namespace Infrastructure.Interface.Branche;

public interface IBrancheCrudService
{
    Task<Response<string>> CreateAsync(BrancheCreateDto dto);
    Task<Response<string>> UpdateAsync(int id, BrancheUpdateDto dto);
    Task<Response<string>> DeleteAsync(int id);
}