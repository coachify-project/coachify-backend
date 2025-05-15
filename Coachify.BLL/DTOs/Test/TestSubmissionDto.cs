namespace Coachify.BLL.DTOs.Test;

public class TestSubmissionDto
{
    public int Id { get; set; }
    public int TestId { get; set; }
    public string TestTitle { get; set; }
    public DateTime SubmittedAt { get; set; }
    public double Score { get; set; }
    public int UserId { get; set; }
}