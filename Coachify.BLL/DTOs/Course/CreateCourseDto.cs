namespace Coachify.BLL.DTOs.Course;

public class CreateCourseDto
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public double Price { get; set; }
    public int MaxClients { get; set; }
    public string CategoryName { get; set; } = null!;
    
    //public int CategoryId { get; set; }
    public int CoachId { get; set; }
}