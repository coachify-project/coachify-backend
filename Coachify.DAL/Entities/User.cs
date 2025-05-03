using System.ComponentModel.DataAnnotations;

namespace Coachify.DAL.Entities;

public class User
{
    [Key]
    public int UserId { get; set; }

    [Required, MaxLength(50)] public string FirstName { get; set; } = null!;
    [Required, MaxLength(50)] public string LastName { get; set; } = null!;
    [Required, MaxLength(50)] public string Email { get; set; } = null!;
    [Required, MaxLength(255)] public string PasswordHash { get; set; } = null!;
    [Required]
    public string PasswordSalt { get; set; }

    public int RoleId { get; set; }
    public Role Role { get; set; } = null!;

    public Coach? CoachProfile { get; set; }
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    public ICollection<Feedback>     Feedbacks    { get; set; } = new List<Feedback>();
    public ICollection<TestSubmission> TestSubmissions { get; set; } = new List<TestSubmission>();
}