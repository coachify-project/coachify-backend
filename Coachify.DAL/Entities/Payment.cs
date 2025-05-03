using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coachify.DAL.Entities;

public class Payment
{
    [Key]
    public int PaymentId { get; set; }

    [ForeignKey(nameof(Enrollment))]
    public int EnrollId { get; set; }
    public Enrollment Enrollment { get; set; } = null!;

    public double Amount { get; set; } 

    public int StatusId { get; set; }
    public PaymentStatus Status { get; set; } = null!;

    [MaxLength(255)] public string? TransactionId { get; set; }

    public DateTime PaidAt { get; set; }
}