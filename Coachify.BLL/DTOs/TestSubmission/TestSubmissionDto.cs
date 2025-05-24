namespace Coachify.BLL.DTOs.TestSubmission;

public class TestSubmissionDto
{
    public int SubmissionId { get; set; }
    public int TestId { get; set; }
    public int UserId { get; set; }
    public DateTime SubmittedAt { get; set; }
    public int Score { get; set; }
    public bool IsPassed { get; set; }
    public int TotalQuestions { get; set; }
    public int CorrectAnswers { get; set; }
}