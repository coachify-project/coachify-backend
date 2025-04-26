using System.ComponentModel.DataAnnotations;

namespace Coachify.DAL.Entities;

public class AnswerOption
{
    [Key]
    public int OptionId { get; set; }

    public int QuestionId { get; set; }
    public Question Question { get; set; }

    [Required]
    public string Text { get; set; }

    public bool IsCorrect { get; set; }
}