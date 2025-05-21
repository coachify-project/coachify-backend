using Coachify.BLL.DTOs.AnswerOption;

namespace Coachify.BLL.Interfaces;

public interface IAnswerOptionService
{
    Task<IEnumerable<AnswerOptionDto>> GetAllAsync();
    Task<AnswerOptionDto?> GetByIdAsync(int id);
    Task<AnswerOptionDto> CreateAsync(CreateAnswerOptionDto dto);
    Task UpdateAsync(int id, UpdateAnswerOptionDto dto);
    Task<bool> DeleteAsync(int id);
}