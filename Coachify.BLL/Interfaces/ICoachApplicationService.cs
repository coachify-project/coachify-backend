using Coachify.BLL.DTOs.CoachApplication;

namespace Coachify.BLL.Interfaces;

public interface ICoachApplicationService
{
    Task<IEnumerable<CoachApplicationDto>> GetAllAsync();
    Task<CoachApplicationDto?> GetByIdAsync(int applicationId);
    Task<IEnumerable<CoachApplicationDto>> GetPendingApplicationsAsync();
    Task<CoachApplicationDto> CreateAsync(CreateCoachApplicationDto dto);
    Task<bool> DeleteAsync(int applicationId);
    Task ApproveCoachApplicationAsync(int applicationId);
    Task RejectCoachApplicationAsync(int applicationId);

}