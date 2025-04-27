using System.ComponentModel.DataAnnotations;

namespace Coachify.DAL.Entities;

public class TestSubmissionAnswer
{
    [Key] public int SubmissionAnswerId { get; set; }
    
    public int SubmissionId { get; set; }
    public TestSubmission Submission { get; set; } = null!;
    
    [Required]
    public int QuestionId { get; set; }
    public Question Question { get; set; } = null!;

    [Required]
    public int OptionId   { get; set; }
    public AnswerOption Option { get; set; } = null!;
}