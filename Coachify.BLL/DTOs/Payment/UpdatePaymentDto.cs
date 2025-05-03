namespace Coachify.API.DTOs.Payment;

public class UpdatePaymentDto
{
    public decimal Amount { get; set; }
    public int StatusId { get; set; }
    public DateTime PaidAt { get; set; }
}