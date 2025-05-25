using Coachify.BLL.DTOs.Lesson;
using Coachify.BLL.DTOs.Progress;
using Coachify.BLL.DTOs.Skill;
using Coachify.BLL.DTOs.Test;

namespace Coachify.BLL.DTOs.Module;

public class ModuleDto
{
    public int ModuleId { get; set; }
    public int CourseId { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public List<LessonDto> Lessons { get; set; } = new();
    public TestDto? Test { get; set; }
    public ProgressStatusDto Status { get; set; } = null!;

    public List<SkillDto> Skills { get; set; } = new();
}