namespace Coachify.BLL.DTOs.Progress;

public class CompletedIdsDto
{
    public List<int> CompletedLessons { get; set; } = new();
    public List<int> CompletedModules { get; set; } = new();
}
