using System.ComponentModel.DataAnnotations;
using Coachify.BLL.DTOs.Lesson;

namespace Coachify.BLL.DTOs.Module;

public class CreateModuleDto
{
    [Required] public int CourseId { get; set; }

    [Required, MaxLength(255)] public string Title { get; set; } = null!;

    [Required, MaxLength(255)] public string Description { get; set; } = null!;

    public int? TestId { get; set; }

    public List<string> SkillNames { get; set; } = new();
}