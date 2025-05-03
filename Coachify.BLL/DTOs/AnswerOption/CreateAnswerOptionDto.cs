namespace Coachify.API.DTOs.AnswerOption;

public class CreateAnswerOptionDto
{
    public string Text { get; set; }
    public bool IsCorrect { get; set; }
    public int QuestionId { get; set; }
}