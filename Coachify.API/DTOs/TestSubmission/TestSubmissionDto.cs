namespace Coachify.API.DTOs.TestSubmission;

public class TestSubmissionDto
{
    public int Id { get; set; }
    public int TestId { get; set; }
    public int UserId { get; set; }
    public DateTime SubmittedAt { get; set; }
}