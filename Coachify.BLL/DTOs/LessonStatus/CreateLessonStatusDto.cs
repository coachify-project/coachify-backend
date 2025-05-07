using System.ComponentModel.DataAnnotations;

namespace Coachify.BLL.DTOs.LessonStatus;

public class CreateLessonStatusDto
{
    [Required]
    public string StatusName { get; set; }
}