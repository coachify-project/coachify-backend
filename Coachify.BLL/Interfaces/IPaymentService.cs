using Coachify.BLL.DTOs.Payment;

namespace Coachify.BLL.Interfaces;

public interface IPaymentService
{
    Task<IEnumerable<PaymentDto>> GetAllAsync();
    Task<PaymentDto?> GetByIdAsync(int id);
    Task<PaymentDto> CreateAsync(CreatePaymentDto dto);
    Task UpdateAsync(int id, UpdatePaymentDto dto);
    Task<bool> DeleteAsync(int id);
}