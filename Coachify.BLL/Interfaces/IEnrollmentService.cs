using Coachify.BLL.DTOs.Enrollment;

namespace Coachify.BLL.Interfaces;

public interface IEnrollmentService
{
    Task<IEnumerable<EnrollmentDto>> GetAllAsync();
    Task<EnrollmentDto?> GetByIdAsync(int id);
    Task<EnrollmentDto> CreateAsync(CreateEnrollmentDto dto);
    Task UpdateAsync(int id, UpdateEnrollmentDto dto);
    Task<bool> DeleteAsync(int id);
}