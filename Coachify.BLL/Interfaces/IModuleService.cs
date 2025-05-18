using System.Reflection;
using Coachify.BLL.DTOs.Module;

namespace Coachify.BLL.Interfaces;

public interface IModuleService
{
    Task<IEnumerable<ModuleDto>> GetAllAsync();

    Task<IEnumerable<ModuleDto>> GetAllByCourseAsync(int courseId);

    Task<ModuleDto?> GetByIdAsync(int id);
    Task<ModuleDto> CreateAsync(CreateModuleDto dto);
    Task<ModuleDto> UpdateAsync(int id, UpdateModuleDto dto);
    Task<bool> DeleteAsync(int id);

    Task<bool> StartModuleAsync(int userId, int moduleId);

}