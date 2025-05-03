namespace Coachify.API.DTOs.Course;

public class CreateCourseDto
{
    public string Title { get; set; }
    public int CoachId { get; set; }
    public int CategoryId { get; set; }
    public int StatusId { get; set; }
}