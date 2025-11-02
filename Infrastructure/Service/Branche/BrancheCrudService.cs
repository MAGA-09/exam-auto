using Domain.DTOS.Branch;
using Infrastructure.ApiResponse;
using Infrastructure.Data;
using Infrastructure.Enum;
using Infrastructure.Interface.Branche;
using Infrastructure.Validator.Branche;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Service.Branche;

public class BrancheCrudService(DataContext _context) : IBrancheCrudService
{
    public async Task<Response<string>> CreateAsync(BrancheCreateDto dto)
    {
        try
        {
            var validation = CreateValidator.ValidateCreate(dto);
            if (validation != null) return validation; 
            
            var exists = await _context.Branches
                .AnyAsync(b => EF.Functions.ILike(b.Name, dto.Name.Trim()));

            if (exists)
                return Response<string>.Fail("Филиал с таким названием уже существует.", ErrorType.Conflict);

            var entity = new Domain.Entities.Branche
            {
                Name = dto.Name.Trim(),
                Location = dto.Location.Trim()
            };

            _context.Branches.Add(entity);
            await _context.SaveChangesAsync();

            return Response<string>.Ok("Филиал успешно создан.");
        }
        catch (Exception ex)
        {
            return Response<string>.Fail($"Ошибка при создании филиала: {ex.Message}", ErrorType.Internal);
        }
    }

    public async Task<Response<string>> UpdateAsync(int id, BrancheUpdateDto dto)
    {
        try
        {
            var validation = UpdateValidator.ValidateUpdate(dto);
            if (validation != null) return validation;
            
            var branch = await _context.Branches.FirstOrDefaultAsync(b => b.Id == id);

            if (branch == null)
                return Response<string>.Fail("Филиал не найден.", ErrorType.NotFound);
            
            branch.Name = dto.Name.Trim();
            branch.Location = dto.Location.Trim();

            await _context.SaveChangesAsync();

            return Response<string>.Ok("Филиал успешно обновлён.");
        }
        catch (Exception ex)
        {
            return Response<string>.Fail($"Ошибка при обновлении филиала: {ex.Message}", ErrorType.Internal);
        }
    }

    public async Task<Response<string>> DeleteAsync(int id)
    {
        try
        {
            if (id <= 0)
                return Response<string>.Fail("Некорректный идентификатор филиала.", ErrorType.Validation);

            var branch = await _context.Branches.FirstOrDefaultAsync(b => b.Id == id);

            if (branch == null)
                return Response<string>.Fail("Филиал не найден.", ErrorType.NotFound);

            _context.Branches.Remove(branch);
            await _context.SaveChangesAsync();

            return Response<string>.Ok("Филиал успешно удалён.");
        }
        catch (Exception ex)
        {
            return Response<string>.Fail($"Ошибка при удалении филиала: {ex.Message}", ErrorType.Internal);
        }
    }
}