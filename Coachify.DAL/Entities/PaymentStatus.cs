using System.ComponentModel.DataAnnotations;

namespace Coachify.DAL.Entities;

public class PaymentStatus
{
    [Key] public int StatusId { get; set; }
    [Required, MaxLength(255)] public string Name { get; set; } = null!;

    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}