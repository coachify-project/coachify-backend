using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Coachify.DAL.Entities;

public class Course
{
    [Key]
    public int CourseId { get; set; }
    [Required]
    public string Title { get; set; }
    public string Description { get; set; }
    public int MaxStudents { get; set; }

    public int CategoryId { get; set; }
    public Category Category { get; set; }

    public int CoachId { get; set; }
    public User Coach { get; set; }

    public CourseStatus Status { get; set; } // Draft, PendingApproval, Published, Blocked
    public DateTime CreatedAt { get; set; }

    public ICollection<Module> Modules { get; set; }
    public ICollection<Enrollment> Enrollments { get; set; }
    public ICollection<Feedback> Feedbacks { get; set; }
    public ICollection<Certificate> Certificates { get; set; }
}