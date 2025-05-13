using Coachify.BLL.DTOs.Enrollment;

namespace Coachify.BLL.Interfaces;

public interface IEnrollmentStatusService
{
    Task<IEnumerable<EnrollmentStatusDto>> GetAllAsync();
}