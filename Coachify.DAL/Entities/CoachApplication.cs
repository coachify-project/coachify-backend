using System.ComponentModel.DataAnnotations;

namespace Coachify.DAL.Entities;

public class CoachApplication
{
    [Key]
    public int ApplicationId { get; set; }

    public int UserId { get; set; }
    public User Applicant { get; set; } = null!;

    public DateTime SubmittedAt { get; set; }
    public int StatusId { get; set; }
    public UserCoachApplicationStatus Status { get; set; } = null!;  // Pending, Approved, Rejected

}