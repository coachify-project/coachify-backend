namespace Coachify.API.DTOs.TestSubmissionAnswer;

public class CreateTestSubmissionAnswerDto
{
    public int SubmissionId { get; set; }
    public int QuestionId { get; set; }
    public int OptionId { get; set; }
}