using System.ComponentModel.DataAnnotations;

namespace Coachify.DAL.Entities;

public class CourseStatus
{
    [Key]
    public int StatusId { get; set; }

    [Required, MaxLength(255)] public string Name { get; set; } = null!;// Draft, PendingApproval, Published, Blocked

    public ICollection<Course> Courses { get; set; } = new List<Course>();
}