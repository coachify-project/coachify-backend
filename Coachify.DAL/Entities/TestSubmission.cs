using System.ComponentModel.DataAnnotations;

namespace Coachify.DAL.Entities;

public class TestSubmission
{
    [Key] public int SubmissionId { get; set; }
    public int TestId { get; set; }
    public Test Test { get; set; } = null!;
    public int UserId { get; set; }
    public User Client { get; set; } = null!;
    [Required] public DateTime SubmittedAt { get; set; }
    public int Score { get; set; }
    public bool IsPassed { get; set; }

    public ICollection<TestSubmissionAnswer> Answers { get; set; } = new List<TestSubmissionAnswer>();
}