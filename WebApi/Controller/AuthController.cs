using Domain.DTOS.Auth;
using Infrastructure.Enum;
using Infrastructure.Interface.Auth;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController(IAuthService service) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginRequestDto dto)
    {
        var result = await service.LoginAsync(dto);
        if (result.IsSuccess)
            return Ok(result.Data);

        return result.ErrorType switch
        {
            ErrorType.UnAuthorized => Unauthorized(new { error = result.Message }),
            ErrorType.Validation => BadRequest(new { error = result.Message }),
            ErrorType.Internal => StatusCode(500, new { error = result.Message }),
            _ => StatusCode(500, new { error = result.Message ?? "Unhandled error" })
        };
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequestDto dto)
    {
        var result = await service.RegisterAsync(dto);
        if (result.IsSuccess)
            return Ok(new { message = result.Message });

        return result.ErrorType switch
        {
            ErrorType.Validation => BadRequest(new { error = result.Message }),
            ErrorType.Conflict => Conflict(new { error = result.Message }),
            ErrorType.Internal => StatusCode(500, new { error = result.Message }),
            _ => StatusCode(500, new { error = result.Message ?? "Unhandled error" })
        };
    }
}