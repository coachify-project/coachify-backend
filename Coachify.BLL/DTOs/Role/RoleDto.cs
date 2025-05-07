using System.ComponentModel.DataAnnotations;

namespace Coachify.BLL.DTOs.Role;

public class RoleDto
{
    [Required]
    public int Id { get; set; }
    [Required]
    public string RoleName { get; set; }
}