using System.Reflection;
using Coachify.BLL.DTOs.Module;
using Coachify.BLL.DTOs.Test;
using Coachify.DAL.Entities;

namespace Coachify.BLL.Interfaces;

public interface IModuleService
{
    Task<IEnumerable<ModuleDto>> GetAllAsync();

    Task<IEnumerable<ModuleDto>> GetAllByCourseAsync(int courseId);

    Task<ModuleDto?> GetByIdAsync(int id);

    Task<ModuleDto?> GetByIdForUserAsync(int moduleId, int userId);

    Task<TestDto?> GetTestByModuleForUserAsync(int userId, int moduleId);

    Task<ModuleDto> CreateAsync(CreateModuleDto dto);

    Task<ModuleDto> UpdateAsync(int id, UpdateModuleDto dto);

    Task<bool> DeleteAsync(int id);

    // Task<bool> StartModuleAsync(int userId, int moduleId);
    //
    // Task<bool> MarkLessonCompletedAsync(int userId, int lessonId);
    //
    // Task<IEnumerable<UserLessonProgress>> GetUserLessonProgressAsync(int userId, int moduleId);
    //
    // Task<bool> CompleteModuleAsync(int userId, int moduleId);

}