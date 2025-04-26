using System.ComponentModel.DataAnnotations;

namespace Coachify.DAL.Entities;

public class Enrollment
{
    [Key]
    public int EnrollmentId { get; set; }

    public int UserId { get; set; }
    public User Student { get; set; }

    public int CourseId { get; set; }
    public Course Course { get; set; }

    public DateTime EnrolledAt { get; set; }
    public bool Completed { get; set; }
}