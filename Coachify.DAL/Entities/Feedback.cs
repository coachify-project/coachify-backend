using System.ComponentModel.DataAnnotations;

namespace Coachify.DAL.Entities;

public class Feedback
{
    [Key]
    public int FeedbackId { get; set; }

    public int UserId { get; set; }
    public User Client { get; set; }

    public int CourseId { get; set; }
    public Course Course { get; set; }

    public string Comment { get; set; }
    public DateTime SubmittedAt { get; set; }
    public FeedbackStatus Status { get; set; } // Pending, Approved, Rejected
}