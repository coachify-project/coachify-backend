using System.ComponentModel.DataAnnotations;

namespace Coachify.DAL.Entities;

public class Enrollment
{
    [Key]
    public int EnrollmentId { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int CourseId { get; set; }
    public Course Course { get; set; } = null!;
    
    public int StatusId { get; set; }
    public EnrollmentStatus Status { get; set; } = null!;
    
    public DateTime? CompletedAt { get; set; }

    public DateTime EnrolledAt { get; set; }
    public int ProgressPercentage { get; set; }
    public bool IsEnrolled { get; set; } //true = active
    
    public Payment? Payment { get; set; }
    public Certificate? Certificate { get; set; }
    public ICollection<TestSubmission> TestSubmissions { get; set; } = new List<TestSubmission>();
}