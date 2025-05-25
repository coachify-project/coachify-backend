using System.ComponentModel.DataAnnotations;

namespace Coachify.BLL.DTOs.CoachApplication
{
    public class CreateCoachApplicationDto
    {
        [Required] public int UserId { get; set; }

        [Required, MaxLength(1000)] public string Bio { get; set; } = null!;

        [Required, MaxLength(255)] public string Specialization { get; set; } = null!;
        
        public string? AvatarUrl { get; set; } = null!;
    }
}