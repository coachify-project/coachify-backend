using Coachify.BLL.DTOs.Module;

namespace Coachify.BLL.Interfaces;

public interface IModuleService
{
    Task<IEnumerable<ModuleDto>> GetAllAsync();
    Task<ModuleDto?> GetByIdAsync(int id);
    Task<ModuleDto> CreateAsync(CreateModuleDto dto);
    Task UpdateAsync(int id, UpdateModuleDto dto);
    Task<bool> DeleteAsync(int id);
}