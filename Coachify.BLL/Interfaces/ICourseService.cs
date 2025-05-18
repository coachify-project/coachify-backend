using Coachify.BLL.DTOs.Course;

namespace Coachify.BLL.Interfaces;

public interface ICourseService
{
    Task<IEnumerable<CourseDto>> GetAllAsync();
    Task<CourseDto?> GetByIdAsync(int id);
    Task<IEnumerable<CourseDto>> GetCoursesForAdminReviewAsync();

    Task<IEnumerable<CourseDto>> GetCoursesByRoleIdAsync(int roleId);
    Task<IEnumerable<CourseDto>> GetCoachCoursesAsync(int coachId);
    Task<IEnumerable<UserCourseDto>> GetCoursesByUserAsync(int userId);
    Task<CourseDto> CreateAsync(CreateCourseDto dto);
    Task UpdateAsync(int id, UpdateCourseDto dto);
    Task<bool> DeleteAsync(int id);

    Task<bool> SubmitCourseAsync(int courseId, int coachId);
    Task<bool> ApproveCourseAsync(int courseId);
    Task<bool> RejectCourseAsync(int courseId);
    Task<bool> StartCourseAsync(int courseId, int userId);

    Task<bool> CompleteCourseAsync(int courseId, int userId);
}