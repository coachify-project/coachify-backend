namespace Coachify.BLL.DTOs.Course;

public class UpdateCourseDto
{
    public string Title { get; set; } = null!;
    public int CategoryId { get; set; }
    
    public string Description { get; set; } = null!;
    public double Price { get; set; }
    public int MaxClients { get; set; } 
}