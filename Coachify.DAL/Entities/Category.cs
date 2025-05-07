using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coachify.DAL.Entities;

public class Category
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CategoryId { get; set; }

    [Required, MaxLength(100)] public string Name { get; set; } = null!;
    [MaxLength(255)] public string? Description { get; set; } 

    public ICollection<Course> Courses { get; set; } = new List<Course>();
}