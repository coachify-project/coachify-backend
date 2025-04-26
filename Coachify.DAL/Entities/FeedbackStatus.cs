using System.ComponentModel.DataAnnotations;

namespace Coachify.DAL.Entities;

public class FeedbackStatus
{
    [Key]
    public int StatusId { get; set; }
    [Required]
    public string Name { get; set; }

    public ICollection<Feedback> Feedbacks { get; set; }
}