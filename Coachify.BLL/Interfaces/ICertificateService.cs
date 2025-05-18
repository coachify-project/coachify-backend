using Coachify.BLL.DTOs.Certificate;

namespace Coachify.BLL.Interfaces;

public interface ICertificateService
{
    //Task<IEnumerable<CertificateDto>> GetAllAsync();
    // Task<CertificateDto?> GetByIdAsync(int id);
    // Task<CertificateDto> CreateAsync(CreateCertificateDto dto);
    // Task UpdateAsync(int id, UpdateCertificateDto dto);
    // Task<bool> DeleteAsync(int id);
    
    Task CreateCertificateForEnrollmentAsync(int enrollmentId);

    Task<string> GenerateCertificatePdfAsync(int certificateId, string FirstName,string LastName, string courseTitle, System.DateTime IssueDate);
    Task<byte[]> GetCertificatePdfAsync(int certificateId);
}