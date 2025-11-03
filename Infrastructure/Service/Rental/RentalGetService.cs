using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.DTOS.Rental;
using Infrastructure.ApiResponse;
using Infrastructure.Data;
using Infrastructure.Enum;
using Infrastructure.Interface.Rental;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Service.Rental;

public class RentalGetService(DataContext _context) : IRentalGetService
{
    public async Task<Response<List<RentalGetDto>>> GetAsync()
    {
        try
        {
            var rentals = await _context.Rentals
                .Select(r => new RentalGetDto
                {
                    Id = r.Id,
                    CarId = r.CarId,
                    CustomerId = r.CustomerId,
                    BranchId = r.BranchId,
                    StartDate = r.StartDate,
                    EndDate = r.EndDate,
                    TotalCost = r.TotalCost
                }).ToListAsync();

            return Response<List<RentalGetDto>>.Ok(rentals);
        }
        catch (Exception ex)
        {
            return Response<List<RentalGetDto>>.Fail($"Ошибка при получении аренд: {ex.Message}", ErrorType.Internal);
        }
    }

    public async Task<Response<RentalGetDto>> GetByIdAsync(int id)
    {
        try
        {
            var r = await _context.Rentals
                .FirstOrDefaultAsync(r => r.Id == id);

            if (r == null)
                return Response<RentalGetDto>.Fail("Аренда не найдена.", ErrorType.NotFound);

            var dto = new RentalGetDto
            {
                Id = r.Id,
                CarId = r.CarId,
                CustomerId = r.CustomerId,
                BranchId = r.BranchId,
                StartDate = r.StartDate,
                EndDate = r.EndDate,
                TotalCost = r.TotalCost
            };

            return Response<RentalGetDto>.Ok(dto);
        }
        catch (Exception ex)
        {
            return Response<RentalGetDto>.Fail($"Ошибка при получении аренды: {ex.Message}", ErrorType.Internal);
        }
    }
}