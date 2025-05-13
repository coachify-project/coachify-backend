using Coachify.BLL.DTOs.CoachApplication;

namespace Coachify.BLL.Interfaces;

public interface ICoachApplicationService
{
    Task<IEnumerable<CoachApplicationDto>> GetAllAsync();
    Task<CoachApplicationDto?> GetByIdAsync(int applicationId);
    Task<CoachApplicationDto> CreateAsync(CreateCoachApplicationDto dto);
    Task UpdateAsync(int id, UpdateCoachApplicationDto dto);
    Task<bool> DeleteAsync(int applicationId);
    Task ApproveCoachApplicationAsync(int applicationId);
    Task<IEnumerable<CoachApplicationDto>> GetPendingApplicationsAsync();

    Task RejectCoachApplicationAsync(int applicationId);

}