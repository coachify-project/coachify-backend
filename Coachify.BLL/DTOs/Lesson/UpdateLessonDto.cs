using System.ComponentModel.DataAnnotations;

namespace Coachify.BLL.DTOs.Lesson;

public class UpdateLessonDto
{
    [Required] public string Title { get; set; } = null!;

    public string? Introduction { get; set; }
    public string? LessonObjectives { get; set; }

    [Required]
    [Url(ErrorMessage = "VideoUrl должен быть корректным URL.")]
    public string VideoUrl { get; set; } = null!;
}