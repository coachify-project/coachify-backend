using Coachify.BLL.DTOs.Module;

namespace Coachify.BLL.DTOs.Course;

public class CourseDto
{
    public int CourseId { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public double Price { get; set; }
    public int MaxClients { get; set; }
    public string? PosterUrl { get; set; }
    
    public DateTime SubmittedAt { get; set; }

    public int CategoryId { get; set; }
    public int CoachId { get; set; }
    public int StatusId { get; set; }
    
    
     public List<ModuleDto> Modules { get; set; } = new();

}