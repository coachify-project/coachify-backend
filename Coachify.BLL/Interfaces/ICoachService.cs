using Coachify.BLL.DTOs.Coach;

namespace Coachify.BLL.Interfaces
{
    public interface ICoachService
    {
        Task<IEnumerable<CoachDto>> GetAllAsync();
        Task<CoachDto?> GetByIdAsync(int id);
        Task<CoachDto> CreateAsync(CreateCoachDto dto);
        Task UpdateAsync(int id, UpdateCoachDto dto);
        Task<bool> DeleteAsync(int id);
    }
}