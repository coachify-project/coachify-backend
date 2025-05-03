namespace Coachify.API.DTOs.Lesson;

public class LessonDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int ModuleId { get; set; }
    public int StatusId { get; set; }
}