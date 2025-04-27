using System.ComponentModel.DataAnnotations;

namespace Coachify.DAL.Entities;

public class Lesson
{
    [Key]
    public int LessonId { get; set; }

    [Required, MaxLength(255)] public string Title { get; set; } = null!;
    [MaxLength(255)] public string? Description { get; set; }

    public int ModuleId { get; set; }
    public Module Module { get; set; } = null!;
    
    [MaxLength(255)] public string? VideoUrl { get; set; }
    [MaxLength(255)] public string? LessonMaterials { get; set; }

    public ICollection<Test> Tests { get; set; } = new List<Test>();
    public int StatusId { get; set; }
    public LessonStatus? Status { get; set; }
}