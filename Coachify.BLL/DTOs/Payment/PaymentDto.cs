namespace Coachify.BLL.DTOs.Payment;

public class PaymentDto
{
    public int Id { get; set; }
    public int? EnrollId { get; set; }
    public decimal Amount { get; set; }
    public int StatusId { get; set; }
    public DateTime PaidAt { get; set; }
}