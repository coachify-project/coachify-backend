using System.ComponentModel.DataAnnotations;

namespace Coachify.BLL.DTOs.User;

public class CreateUserDto
{
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
    public int RoleId { get; set; }
}