using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coachify.DAL.Entities;

public class Certificate
{
    [Key] public int CertificateId { get; set; }

    [ForeignKey(nameof(Enrollment))]
    public int EnrollmentId { get; set; }
    public Enrollment Enrollment { get; set; } = null!;

    public DateTime IssueDate     { get; set; }
    [MaxLength(255)] public string CertificateUrl { get; set; } = null!;
    public int CertificateNum { get; set; }
    [MaxLength(255)] public string CourseTitle    { get; set; } = null!;
}