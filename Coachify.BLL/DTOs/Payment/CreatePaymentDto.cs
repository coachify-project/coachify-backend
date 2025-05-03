namespace Coachify.BLL.DTOs.Payment;

public class CreatePaymentDto
{
    public decimal Amount { get; set; }
    public int StatusId { get; set; }
    public int? EnrollId { get; set; }
    public DateTime PaidAt { get; set; }
}