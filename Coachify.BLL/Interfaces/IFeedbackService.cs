using Coachify.BLL.DTOs.Feedback;

namespace Coachify.BLL.Interfaces;

public interface IFeedbackService
{
    Task<IEnumerable<FeedbackDto>> GetAllAsync();
    Task<FeedbackDto?> GetByIdAsync(int id);
    Task<FeedbackDto> CreateAsync(CreateFeedbackDto dto);
    Task UpdateAsync(int id, UpdateFeedbackDto dto);
    Task<bool> DeleteAsync(int id);
}