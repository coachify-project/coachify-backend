using System.ComponentModel.DataAnnotations;

namespace Coachify.BLL.DTOs.Course;

public class UpdateCourseDto
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    [Range(0, double.MaxValue)] public double Price { get; set; }

    [Range(1, int.MaxValue)] public int MaxClients { get; set; }

    [Required] public string CategoryName { get; set; } = null!;

    [Url] public string? PosterUrl { get; set; }
}