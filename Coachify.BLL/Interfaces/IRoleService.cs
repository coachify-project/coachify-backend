using Coachify.BLL.DTOs.Role;

namespace Coachify.BLL.Interfaces;

public interface IRoleService
{
    Task<IEnumerable<RoleDto>> GetAllAsync();
    Task<RoleDto?> GetByIdAsync(int id);
    Task<RoleDto> CreateAsync(CreateRoleDto dto);
    Task UpdateAsync(int id, UpdateRoleDto dto);
    Task<bool> DeleteAsync(int id);
}