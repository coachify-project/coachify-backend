using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coachify.DAL.Entities;

public class CoachProfile
{
    [Key, ForeignKey("Applicant")]
    public int UserId { get; set; }
    public User Applicant { get; set; }

    public string Specialization { get; set; }
    public string About { get; set; }
}