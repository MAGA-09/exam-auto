using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.DTOS.Auth;
using Infrastructure.ApiResponse;
using Infrastructure.Data;
using Infrastructure.Enum;
using Infrastructure.Interface.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Service.Auth;

public class AuthService(DataContext context,
    IHttpContextAccessor httpContextAccessor,
    IConfiguration configuration) : IAuthService
{
    public async Task<Response<LoginResponseDto>> LoginAsync(LoginRequestDto dto)
    {
        var user = await context.Customers.FirstOrDefaultAsync(u => u.Phone == dto.UserName);
        if (user == null)
            return Response<LoginResponseDto>.Fail("Username or password incorrect.", ErrorType.UnAuthorized);

        bool verify = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
        if (!verify)
            return Response<LoginResponseDto>.Fail("Username or password incorrect.", ErrorType.UnAuthorized);

        var token = GenerateJwtToken(user);

        httpContextAccessor.HttpContext!.Response.Cookies.Append("access_token", token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddHours(1)
        });

        return Response<LoginResponseDto>.Ok(new LoginResponseDto
        {
            Id = user.Id.ToString(),
            UserName = user.Phone,
            Token = token
        });
    }

    private string GenerateJwtToken(Domain.Entities.Customer user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.MobilePhone, user.Phone)
        };

        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(60),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<Response<string>> RegisterAsync(RegisterRequestDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.UserName))
            return Response<string>.Fail("Username is required.", ErrorType.Validation);

        if (string.IsNullOrWhiteSpace(dto.Password))
            return Response<string>.Fail("Password is required", ErrorType.Validation);

        if (dto.Password.Length < 4)
            return Response<string>.Fail("Password too short", ErrorType.Validation);

        if (dto.Password != dto.ConfirmPassword)
            return Response<string>.Fail("Passwords do not match", ErrorType.Validation);

        bool exists = await context.Customers.AnyAsync(u => u.Phone == dto.UserName);
        if (exists)
            return Response<string>.Fail("User already exists", ErrorType.Conflict);

        var hashed = BCrypt.Net.BCrypt.HashPassword(dto.Password);

        var newCustomer = new Domain.Entities.Customer
        {
            Phone = dto.UserName,
            PasswordHash = hashed,
            FullName = ""
        };

        context.Customers.Add(newCustomer);
        await context.SaveChangesAsync();

        return Response<string>.Ok("User registered successfully");
    }
}
