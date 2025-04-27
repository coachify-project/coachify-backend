using System.ComponentModel.DataAnnotations;

namespace Coachify.DAL.Entities;

public class FeedbackStatus
{
    [Key]
    public int StatusId { get; set; }

    [Required, MaxLength(255)] public string Name { get; set; } = null!; //PendingApproval, Published, Rejected

    public ICollection<Feedback> Feedbacks { get; set; }
}