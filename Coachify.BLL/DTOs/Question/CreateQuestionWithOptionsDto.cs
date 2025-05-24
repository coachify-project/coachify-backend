using Coachify.BLL.DTOs.AnswerOption;
using System.Collections.Generic;

namespace Coachify.BLL.DTOs.Question;

public class CreateQuestionWithOptionsDto
{
    public string Text { get; set; }
    public List<CreateAnswerOptionDto> Options { get; set; } = new();
}