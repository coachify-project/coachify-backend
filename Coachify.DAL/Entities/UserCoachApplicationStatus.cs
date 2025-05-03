using System.ComponentModel.DataAnnotations;

namespace Coachify.DAL.Entities;

public class UserCoachApplicationStatus
{
    [Key]
    public int StatusId { get; set; }

    [Required, MaxLength(25)] public string Name { get; set; } = null!; // Pending, Approved, Rejected

    public ICollection<CoachApplication> Applications { get; set; } = new List<CoachApplication>();
}