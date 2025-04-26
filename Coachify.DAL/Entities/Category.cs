using System.ComponentModel.DataAnnotations;

namespace Coachify.DAL.Entities;

public class Category
{
    [Key]
    public int CategoryId { get; set; }
    [Required]
    public string Name { get; set; }

    public ICollection<Course> Courses { get; set; }
}