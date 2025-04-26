using System.ComponentModel.DataAnnotations;

namespace Coachify.DAL.Entities;

public class Certificate
{
    [Key]
    public int CertificateId { get; set; }

    public int UserId { get; set; }
    public User Student { get; set; }

    public int CourseId { get; set; }
    public Course Course { get; set; }

    public DateTime IssuedAt { get; set; }
    public string CourseTitle { get; set; }
}