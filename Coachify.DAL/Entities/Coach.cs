using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coachify.DAL.Entities;

public class Coach
{
    [Key, ForeignKey(nameof(User))]
    public int CoachId { get; set; }  
     
    [MaxLength(255)]  public string? Bio            { get; set; }
    [MaxLength(255)]  public string? Specialization { get; set; }
    public bool       Verified      { get; set; }
    public User User { get; set; } = null!;
    public ICollection<Course> Courses { get; set; } = new List<Course>();
    
}