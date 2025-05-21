using System.ComponentModel.DataAnnotations;

namespace Coachify.BLL.DTOs.User;

public class UpdateUserDto
{
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    public string? CurrentPassword { get; set; }
    public string? NewPassword { get; set; }
}