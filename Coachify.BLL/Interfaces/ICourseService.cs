using Coachify.BLL.DTOs.Course;

namespace Coachify.BLL.Interfaces;

public interface ICourseService
{
    Task<IEnumerable<CourseDto>> GetAllAsync();
    Task<CourseDto?> GetByIdAsync(int id);
    Task<CourseDto> CreateAsync(CreateCourseDto dto);
    Task UpdateAsync(int id, UpdateCourseDto dto);
    Task<bool> DeleteAsync(int id);
}