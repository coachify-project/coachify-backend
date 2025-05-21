using Coachify.BLL.DTOs.UserCoachApplicationStatus;

namespace Coachify.BLL.Interfaces;

public interface IUserCoachApplicationStatusService
{
    Task<IEnumerable<UserCoachApplicationStatusDto>> GetAllAsync();
    Task<UserCoachApplicationStatusDto?> GetByIdAsync(int id);
    Task<UserCoachApplicationStatusDto> CreateAsync(CreateUserCoachApplicationStatusDto dto);
    Task UpdateAsync(int id, UpdateUserCoachApplicationStatusDto dto);
    Task<bool> DeleteAsync(int id);
}