using System.ComponentModel.DataAnnotations;

namespace Coachify.DAL.Entities;

public class Certificate
{
    [Key] public int CertificateId { get; set; }

    public int EnrollmentId { get; set; }
    public Enrollment Enrollment { get; set; } = null!;

    public DateTime IssueDate { get; set; }

    [Required, MaxLength(255)] public string Title { get; set; } = null!;

    [MaxLength(500)] public string? Description { get; set; }

    [MaxLength(500)] public string? FilePath { get; set; }
}