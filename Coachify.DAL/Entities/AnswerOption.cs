using System.ComponentModel.DataAnnotations;

namespace Coachify.DAL.Entities;

public class AnswerOption
{
    [Key]
    public int OptionId { get; set; }

    public int QuestionId { get; set; }
    public Question Question { get; set; } = null!;

    [Required, MaxLength(255)] public string Text { get; set; } = null!;

    public bool IsCorrect { get; set; }
    public IEnumerable<TestSubmissionAnswer>? TestSubmissionAnswers { get; set; }
}