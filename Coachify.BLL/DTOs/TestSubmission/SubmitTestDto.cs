namespace Coachify.BLL.DTOs.TestSubmission;

public class SubmitTestDto
{
    public int TestId { get; set; }
    public int UserId { get; set; }

    public List<SubmitAnswerDto> Answers { get; set; } = new();
}