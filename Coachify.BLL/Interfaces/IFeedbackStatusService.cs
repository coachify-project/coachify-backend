using Coachify.BLL.DTOs.FeedbackStatus;

namespace Coachify.BLL.Interfaces;

public interface IFeedbackStatusService
{
    Task<IEnumerable<FeedbackStatusDto>> GetAllAsync();
    Task<FeedbackStatusDto?> GetByIdAsync(int id);
    Task<FeedbackStatusDto> CreateAsync(CreateFeedbackStatusDto dto);
    Task UpdateAsync(int id, UpdateFeedbackStatusDto dto);
    Task<bool> DeleteAsync(int id);
}