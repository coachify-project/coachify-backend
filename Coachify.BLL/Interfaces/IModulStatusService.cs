using Coachify.BLL.DTOs.ModuleStatus;

namespace Coachify.BLL.Interfaces;

public interface IModulStatusService
{
    Task<IEnumerable<ModuleStatusDto>> GetAllAsync();
    Task<ModuleStatusDto?> GetByIdAsync(int id);
    Task<ModuleStatusDto> CreateAsync(CreateModuleStatusDto dto);
    Task UpdateAsync(int id, UpdateModuleStatusDto dto);
    Task<bool> DeleteAsync(int id);
}