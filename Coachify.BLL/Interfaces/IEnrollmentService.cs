using Coachify.BLL.DTOs.Course;
using Coachify.BLL.DTOs.Enrollment;
using Coachify.BLL.Services;

namespace Coachify.BLL.Interfaces;

public interface IEnrollmentService
{
    Task<IEnumerable<EnrollmentDto>> GetAllAsync();
    Task<EnrollmentDto?> GetByIdAsync(int id);
    Task<EnrollmentDto> CreateAsync(CreateEnrollmentDto dto);
    Task UpdateAsync(int id, UpdateEnrollmentDto dto);
    Task<bool> DeleteAsync(int id);

    Task<bool> EnrollUserAsync(int courseId, int userId);
    Task<EnrollmentDto> StartCourseAsync(int courseId, int userId);

    Task CompleteEnrollmentAsync(int enrollmentId);
    Task<bool> CompleteCourseAsync(int courseId, int userId);
}