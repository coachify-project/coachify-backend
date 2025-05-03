namespace Coachify.API.DTOs.Lesson;

public class CreateLessonDto
{
    public string Title { get; set; }
    public int ModuleId { get; set; }
    public int StatusId { get; set; }
}