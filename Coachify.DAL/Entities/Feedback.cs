using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coachify.DAL.Entities;

public class Feedback
{
    [Key]
    public int FeedbackId { get; set; }
    
    [ForeignKey(nameof(Course))]
    public int CourseId { get; set; }
    public Course Course { get; set; } = null!;
    
    [ForeignKey(nameof(Client))]
    public int UserId { get; set; }
    public User Client { get; set; } = null!;

    [Required]
    public string Text { get; set; } = null!;
    
    public DateTime SubmittedAt { get; set; }
    
    public int? Rating { get; set; } 
    
    public int StatusId { get; set; }
    public FeedbackStatus Status { get; set; } = null!; // Pending, Approved, Rejected
}