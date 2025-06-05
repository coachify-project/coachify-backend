using Coachify.BLL.DTOs.Question;

namespace Coachify.BLL.DTOs.Test;

public class TestDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int ModuleId { get; set; }
    
    public List<QuestionDto> Questions { get; set; } = new();
}