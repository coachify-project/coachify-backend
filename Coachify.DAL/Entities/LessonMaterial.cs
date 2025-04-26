using System.ComponentModel.DataAnnotations;

namespace Coachify.DAL.Entities;

public class LessonMaterial
{
    [Key]
    public int MaterialId { get; set; }
    [Required]
    public string Title { get; set; }
    public string Url { get; set; } // video URL or file path

    public int LessonId { get; set; }
    public Lesson Lesson { get; set; }
}