using System.ComponentModel.DataAnnotations;

namespace Coachify.DAL.Entities;

public class Test
{
    [Key]
    public int TestId { get; set; }

    public int ModuleId { get; set; }
    public Module Module { get; set; }

    [Required]
    public string Title { get; set; }

    public ICollection<Question> Questions { get; set; }
}