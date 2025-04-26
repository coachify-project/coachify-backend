using System.ComponentModel.DataAnnotations;

namespace Coachify.DAL.Entities;

public class CoachApplication
{
    [Key]
    public int ApplicationId { get; set; }

    public int UserId { get; set; }
    public User Applicant { get; set; }

    public DateTime SubmittedAt { get; set; }
    public ApplicationStatus Status { get; set; } // Pending, Approved, Rejected

}