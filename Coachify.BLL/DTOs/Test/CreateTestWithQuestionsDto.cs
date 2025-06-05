using Coachify.BLL.DTOs.Question;
using System.Collections.Generic;

namespace Coachify.BLL.DTOs.Test;

public class CreateTestWithQuestionsDto
{
    public string Title { get; set; }
    public int ModuleId { get; set; }
    public List<CreateQuestionWithOptionsDto> Questions { get; set; } = new();
}