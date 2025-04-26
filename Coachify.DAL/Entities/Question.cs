using System.ComponentModel.DataAnnotations;

namespace Coachify.DAL.Entities;

public class Question
{
    [Key]
    public int QuestionId { get; set; }

    public int TestId { get; set; }
    public Test Test { get; set; }

    [Required]
    public string Text { get; set; }

    public ICollection<AnswerOption> Options { get; set; }
}