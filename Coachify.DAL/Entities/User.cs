using System.ComponentModel.DataAnnotations;

namespace Coachify.DAL.Entities;

public class User
{
    [Key]
    public int UserId { get; set; }
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string PasswordHash { get; set; }
    [Required]
    public string PasswordSalt { get; set; }

    public int RoleId { get; set; }
    public Role Role { get; set; }

    // Coach applications and profile
    public ICollection<CoachApplication> CoachApplications { get; set; }
    public CoachProfile CoachProfile { get; set; }

    // Enrollments, reviews, feedbacks, certificates
    public ICollection<Enrollment> Enrollments { get; set; }
    public ICollection<Feedback> Feedbacks { get; set; }
    public ICollection<Certificate> Certificates { get; set; }
    public ICollection<TestSubmission> TestSubmissions { get; set; }
}