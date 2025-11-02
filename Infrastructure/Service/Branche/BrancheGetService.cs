using Domain.DTOS.Branch;
using Infrastructure.ApiResponse;
using Infrastructure.Data;
using Infrastructure.Enum;
using Infrastructure.Interface.Branche;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Service.Branche;

public class BrancheGetService(DataContext _context) : IBrancheGetService
{
    public async Task<Response<List<BrancheGetDto>>> GetAsync()
    {
        try
        {
            var items = await _context.Branches
                .Select(b => new BrancheGetDto
                {
                    Id = b.Id,
                    Name = b.Name,
                    Location = b.Location
                }).ToListAsync();

            return Response<List<BrancheGetDto>>.Ok(items);
        }
        catch (Exception ex)
        {
            return Response<List<BrancheGetDto>>.Fail($"Ошибка при получении филиалов: {ex.Message}", ErrorType.Internal);
        }
    }

    public async Task<Response<BrancheGetDto>> GetByIdAsync(int id)
    {
        try
        {
            if (id <= 0)
                return Response<BrancheGetDto>.Fail("Некорректный идентификатор филиала.", ErrorType.Validation);

            var b = await _context.Branches
                .FirstOrDefaultAsync(x => x.Id == id);

            if (b == null)
                return Response<BrancheGetDto>.Fail("Филиал не найден.", ErrorType.NotFound);

            var dto = new BrancheGetDto
            {
                Id = b.Id,
                Name = b.Name,
                Location = b.Location
            };

            return Response<BrancheGetDto>.Ok(dto);
        }
        catch (Exception ex)
        {
            return Response<BrancheGetDto>.Fail($"Ошибка при получении филиала: {ex.Message}", ErrorType.Internal);
        }
    }
}