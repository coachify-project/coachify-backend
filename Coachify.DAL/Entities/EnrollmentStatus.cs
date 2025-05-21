using System.ComponentModel.DataAnnotations;

namespace Coachify.DAL.Entities;

public class EnrollmentStatus
{
    [Key]
    public int StatusId { get; set; }
    public string Name { get; set; } = null!;
    
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

}