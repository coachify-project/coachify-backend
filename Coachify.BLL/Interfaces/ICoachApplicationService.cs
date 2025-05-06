using Coachify.BLL.DTOs.CoachApplication;

namespace Coachify.BLL.Interfaces;

public interface ICoachApplicationService
{
    Task<IEnumerable<CoachApplicationDto>> GetAllAsync();
    Task<CoachApplicationDto?> GetByIdAsync(int id);
    Task<CoachApplicationDto> CreateAsync(CreateCoachApplicationDto dto);
    Task UpdateAsync(int id, UpdateCoachApplicationDto dto);
    Task<bool> DeleteAsync(int id);
}