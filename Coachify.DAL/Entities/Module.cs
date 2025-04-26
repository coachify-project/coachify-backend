using System.ComponentModel.DataAnnotations;

namespace Coachify.DAL.Entities;

public class Module
{
    [Key]
    public int ModuleId { get; set; }
    [Required]
    public string Title { get; set; }

    public int CourseId { get; set; }
    public Course Course { get; set; }

    public ICollection<Lesson> Lessons { get; set; }
    public Test Test { get; set; }
}