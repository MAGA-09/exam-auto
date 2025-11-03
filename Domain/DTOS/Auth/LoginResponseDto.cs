namespace Domain.DTOS.Auth;

public class LoginResponseDto
{
    public string Id { get; set; }
    public string? UserName { get; set; } = "";
    public string Token { get; set; } = "";
}