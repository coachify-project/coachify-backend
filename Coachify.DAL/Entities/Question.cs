using System.ComponentModel.DataAnnotations;

namespace Coachify.DAL.Entities;

public class Question
{
    [Key]
    public int QuestionId { get; set; }

    public int TestId { get; set; }
    public Test Test { get; set; } = null!;

    [Required, MaxLength(255)] public string Text { get; set; } = null!;

    public ICollection<AnswerOption> Options { get; set; } = new List<AnswerOption>();
    public IEnumerable<TestSubmissionAnswer>? TestSubmissionAnswers { get; set; }
}