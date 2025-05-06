using Coachify.BLL.DTOs.Certificate;

namespace Coachify.BLL.Interfaces;

public interface ICertificateService
{
    Task<IEnumerable<CertificateDto>> GetAllAsync();
    Task<CertificateDto?> GetByIdAsync(int id);
    Task<CertificateDto> CreateAsync(CreateCertificateDto dto);
    Task UpdateAsync(int id, UpdateCertificateDto dto);
    Task<bool> DeleteAsync(int id);
}