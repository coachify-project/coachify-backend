namespace Coachify.API.DTOs.Course;

public class CourseDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int CoachId { get; set; }
    public int CategoryId { get; set; }
    public int StatusId { get; set; }
}