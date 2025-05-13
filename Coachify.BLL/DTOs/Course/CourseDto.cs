namespace Coachify.BLL.DTOs.Course;

public class CourseDto
{
    public int CourseId { get; set; }
    public string Title { get; set; } = null!;
    public int CoachId { get; set; }
    public int CategoryId { get; set; }
    public int StatusId { get; set; }
}