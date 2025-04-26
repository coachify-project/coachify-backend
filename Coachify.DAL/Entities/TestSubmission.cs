using System.ComponentModel.DataAnnotations;

namespace Coachify.DAL.Entities;

public class TestSubmission
{
    [Key]
    public int SubmissionId { get; set; }

    public int TestId { get; set; }
    public Test Test { get; set; }

    public int UserId { get; set; }
    public User Student { get; set; }

    public DateTime SubmittedAt { get; set; }
    public double Score { get; set; }
    public bool Passed { get; set; }
}