using System.ComponentModel.DataAnnotations;

namespace Coachify.BLL.DTOs.UserCoachApplicationStatus;

public class CreateUserCoachApplicationStatusDto
{
    [Required]
    public int StatusId { get; } 
    [Required]
    public string Name { get; set; }
}