using Coachify.BLL.DTOs.PaymentStatus;

namespace Coachify.BLL.Interfaces;

public interface IPaymentStatusService
{
    Task<IEnumerable<PaymentStatusDto>> GetAllAsync();
    Task<PaymentStatusDto?> GetByIdAsync(int id);
    Task<PaymentStatusDto> CreateAsync(CreatePaymentStatusDto dto);
    Task UpdateAsync(int id, UpdatePaymentStatusDto dto);
    Task<bool> DeleteAsync(int id);
}