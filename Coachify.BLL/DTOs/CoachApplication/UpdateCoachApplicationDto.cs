using System.ComponentModel.DataAnnotations;

namespace Coachify.BLL.DTOs.CoachApplication
{
    public class UpdateCoachApplicationDto
    {
        [MaxLength(1000)] public string? Bio { get; set; }

        [MaxLength(255)] public string? Specialization { get; set; }
        [Range(1, 3)] public int? StatusId { get; set; }
    }
}