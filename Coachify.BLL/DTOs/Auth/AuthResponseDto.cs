namespace Coachify.BLL.DTOs.Auth;

public class AuthResponseDto
{
    public string Token     { get; set; } = null!;
    public DateTime ExpiresAt { get; set; }
}