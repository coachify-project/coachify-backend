namespace Coachify.BLL.DTOs.AnswerOption;

public class UpdateAnswerOptionDto
{
    public string Text { get; set; }
    public bool IsCorrect { get; set; }
    public int QuestionId { get; set; }
    
}