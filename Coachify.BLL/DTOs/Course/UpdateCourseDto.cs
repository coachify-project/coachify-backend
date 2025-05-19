namespace Coachify.BLL.DTOs.Course;

public class UpdateCourseDto
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public double Price { get; set; }
    public int MaxClients { get; set; } 
    public string CategoryName{ get; set; }

}