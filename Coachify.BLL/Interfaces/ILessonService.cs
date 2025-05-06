using Coachify.BLL.DTOs.Lesson;

namespace Coachify.BLL.Interfaces;

public interface ILessonService
{
    Task<IEnumerable<LessonDto>> GetAllAsync();
    Task<LessonDto?> GetByIdAsync(int id);
    Task<LessonDto> CreateAsync(CreateLessonDto dto);
    Task UpdateAsync(int id, UpdateLessonDto dto);
    Task<bool> DeleteAsync(int id);
}