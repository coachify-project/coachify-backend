namespace Coachify.BLL.DTOs.Module;

public class UpdateModuleDto
{
    public string Title { get; set; }
    public int StatusId { get; set; }
    
    public List<string> SkillNames { get; set; } = new();

}