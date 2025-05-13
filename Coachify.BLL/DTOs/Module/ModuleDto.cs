namespace Coachify.BLL.DTOs.Module;

public class ModuleDto
{
    public int ModuleId { get; set; }
    public string Title { get; set; }
    public int CourseId { get; set; }
    public int StatusId { get; set; }
    
    public List<string> SkillNames { get; set; } = new();

}