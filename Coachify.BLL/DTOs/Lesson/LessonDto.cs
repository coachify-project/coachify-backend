using Coachify.BLL.DTOs.Progress;

namespace Coachify.BLL.DTOs.Lesson;

public class LessonDto
{
    public int LessonId { get; set; }
    public string Title { get; set; }
    public string? Introduction { get; set; }
    public string? LessonObjectives { get; set; }
    public string VideoUrl { get; set; }
    public int ModuleId { get; set; }
    public ProgressStatusDto Status { get; set; } = null!;
}