using Coachify.BLL.DTOs.LessonStatus;

namespace Coachify.BLL.Interfaces;

public interface ILessonStatusService
{
    Task<IEnumerable<LessonStatusDto>> GetAllAsync();
    Task<LessonStatusDto?> GetByIdAsync(int id);
    Task<LessonStatusDto> CreateAsync(CreateLessonStatusDto dto);
    Task UpdateAsync(int id, UpdateLessonStatusDto dto);
    Task<bool> DeleteAsync(int id);
}