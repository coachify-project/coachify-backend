namespace Coachify.API.DTOs.Module;

public class CreateModuleDto
{
    public string Title { get; set; }
    public int CourseId { get; set; }
    public int StatusId { get; set; }
}