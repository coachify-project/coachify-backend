using System.ComponentModel.DataAnnotations;

namespace Coachify.DAL.Entities;

public class CourseStatus
{
    [Key]
    public int StatusId { get; set; }
    [Required]
    public string Name { get; set; } // Draft, PendingApproval, Published, Blocked

    public ICollection<Course> Courses { get; set; }
}