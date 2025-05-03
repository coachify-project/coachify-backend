using System.ComponentModel.DataAnnotations;

namespace Coachify.DAL.Entities;

public class Category
{
    [Key]
    public int CategoryId { get; set; }

    [Required, MaxLength(100)] public string Name { get; set; } = null!;
    [MaxLength(255)] public string? Description { get; set; } 

    public ICollection<Course> Courses { get; set; } = new List<Course>();
}