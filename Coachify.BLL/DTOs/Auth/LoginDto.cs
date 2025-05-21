using System.ComponentModel.DataAnnotations;

namespace Coachify.BLL.DTOs.Auth;

public class LoginDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;
    [Required]
    public string Password { get; set; } = null!;
}