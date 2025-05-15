namespace Coachify.BLL.DTOs.TestSubmission;

public class SubmitAnswerDto
{
    public int QuestionId { get; set; }
    public List<int> SelectedOptionIds { get; set; }
    
}