using System.ComponentModel.DataAnnotations;

namespace Coachify.DAL.Entities;

public class ApplicationStatus
{
    [Key]
    public int StatusId { get; set; }
    [Required]
    public string Name { get; set; } // Pending, Approved, Rejected

    public ICollection<CoachApplication> Applications { get; set; }
}