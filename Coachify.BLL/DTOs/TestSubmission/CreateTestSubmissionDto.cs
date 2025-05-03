namespace Coachify.API.DTOs.TestSubmission;

public class CreateTestSubmissionDto
{
    public int TestId { get; set; }
    public int UserId { get; set; }
    public DateTime SubmittedAt { get; set; }
}