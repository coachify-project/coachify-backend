using System.ComponentModel.DataAnnotations;

namespace Coachify.DAL.Entities;

public class Test
{
    [Key]
    public int TestId { get; set; }
    public int ModuleId { get; set; }
    public Module Module { get; set; } 
    
    [Required, MaxLength(255)] public string Title { get; set; } = null!;
    public int MaxScore { get; set; }
    public int PassScore { get; set; }
    [MaxLength(255)] public string? Description { get; set; } 
    
    public ICollection<Question> Questions { get; set; } = new List<Question>();
    public ICollection<TestSubmission> Submissions { get; set; } = new List<TestSubmission>();
}