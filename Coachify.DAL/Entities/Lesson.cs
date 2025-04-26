using System.ComponentModel.DataAnnotations;

namespace Coachify.DAL.Entities;

public class Lesson
{
    [Key]
    public int LessonId { get; set; }
    [Required]
    public string Title { get; set; }
    public string Description { get; set; }

    public int ModuleId { get; set; }
    public Module Module { get; set; }

    public ICollection<LessonMaterial> Materials { get; set; }
}