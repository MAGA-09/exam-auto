using Domain.DTOS.Auth;
using Infrastructure.ApiResponse;

namespace Infrastructure.Interface.Auth;

public interface IAuthService
{
    Task<Response<string>> RegisterAsync(RegisterRequestDto dto);
    Task<Response<LoginResponseDto>> LoginAsync(LoginRequestDto dto);
}