// ILessonService.cs (обновленный)
using System.Collections.Generic;
using System.Threading.Tasks;
using Coachify.BLL.DTOs.Lesson;

namespace Coachify.BLL.Interfaces
{
    public interface ILessonService
    {
        Task<IEnumerable<LessonDto>> GetAllAsync();
        Task<LessonDto?> GetByIdAsync(int id);
        Task<IEnumerable<LessonDto>> GetByModuleAsync(int moduleId);
        Task<IEnumerable<LessonDto>> GetByModuleForUserAsync(int moduleId, int userId);
        Task<LessonDto> CreateAsync(CreateLessonDto dto);
        Task<LessonDto> UpdateAsync(int id, UpdateLessonDto dto);
        Task<bool> DeleteAsync(int id);
    }
}