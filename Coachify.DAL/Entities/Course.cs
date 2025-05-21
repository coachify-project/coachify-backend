using System.ComponentModel.DataAnnotations;

namespace Coachify.DAL.Entities;

public class Course
{
    [Key]
    public int CourseId { get; set; }

    [Required, MaxLength(255)] public string Title { get; set; } = null!;
    [MaxLength(255)] public string? Description { get; set; }
    
    public double Price { get; set; }
    public int MaxClients { get; set; }
    public int? Rating { get; set; }
    
    public string? PosterUrl { get; set; }

    public int CoachId { get; set; }
    public Coach Coach { get; set; } = null!;
    
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
    
    public int StatusId { get; set; }
    public CourseStatus Status { get; set; } = null!; // Draft, PendingApproval, Published, Blocked
    
    public ICollection<Module> Modules { get; set; } = new List<Module>();
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    public ICollection<Feedback> Feedbacks { get; set; }  = new List<Feedback>();
}