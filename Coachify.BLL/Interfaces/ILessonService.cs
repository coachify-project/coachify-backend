using Coachify.BLL.DTOs.Lesson;

namespace Coachify.BLL.Interfaces;

public interface ILessonService
{
    Task<IEnumerable<LessonDto>> GetAllAsync();
    Task<LessonDto?> GetByIdAsync(int id);
    Task<LessonDto> CreateAsync(CreateLessonDto dto);
    Task<bool> StartLessonAsync(int userId, int lessonId);
    Task<bool> CompleteLessonAsync(int userId, int lessonId);

    Task<LessonDto> UpdateAsync(int id, UpdateLessonDto dto);
    Task<bool> DeleteAsync(int id);
}