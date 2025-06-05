using System.ComponentModel.DataAnnotations;

namespace Coachify.BLL.DTOs.Module;

public class UpdateModuleDto
{
    [Required, MaxLength(255)] public string Title { get; set; } = null!;

    [Required, MaxLength(255)] public string Description { get; set; } = null!;

    public int? TestId { get; set; }

    public List<string> SkillNames { get; set; } = new();
}