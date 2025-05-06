using Coachify.BLL.DTOs.CourseStatus;

namespace Coachify.BLL.Interfaces;

public interface ICourseStatusService
{
    Task<IEnumerable<CourseStatusDto>> GetAllAsync();
    Task<CourseStatusDto?> GetByIdAsync(int id);
    Task<CourseStatusDto> CreateAsync(CreateCourseStatusDto dto);
    Task UpdateAsync(int id, UpdateCourseStatusDto dto);
    Task<bool> DeleteAsync(int id);
}