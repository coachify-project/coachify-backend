using Coachify.BLL.DTOs.Lesson;
using Coachify.BLL.DTOs.Skill;

namespace Coachify.BLL.DTOs.Module;

public class ModuleDto
{
    public int ModuleId { get; set; }
    public int CourseId { get; set; }
    public int StatusId { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; }


    public List<SkillDto> Skills { get; set; } = new();
    public List<LessonDto> Lessons { get; set; } = new();

}