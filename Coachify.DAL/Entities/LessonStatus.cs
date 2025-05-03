using System.ComponentModel.DataAnnotations;

namespace Coachify.DAL.Entities;

public class LessonStatus
{
    [Key] public int StatusId { get; set; }
    [Required, MaxLength(25)] public string StatusName { get; set; } = null!;

    public ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
}