namespace Coachify.BLL.DTOs.AnswerOption;

public class AnswerOptionDto
{
    public int OptionId { get; set; }
    public int QuestionId  { get; set; }
    public string Text { get; set; }
    public bool IsCorrect { get; set; }
}