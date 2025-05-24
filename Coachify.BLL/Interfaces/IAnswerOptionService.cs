using Coachify.BLL.DTOs.AnswerOption;

namespace Coachify.BLL.Interfaces;

public interface IAnswerOptionService
{
    Task<IEnumerable<AnswerOptionDto>> GetAllAsync();

    Task<AnswerOptionDto?> GetByIdAsync(int id);

    Task<IEnumerable<AnswerOptionDto>> GetByQuestionIdAsync(int questionId);
    Task<AnswerOptionDto> CreateAsync(CreateAnswerOptionDto dto);
    Task<AnswerOptionDto> UpdateAsync(int optionId, UpdateAnswerOptionDto dto);
    Task<bool> DeleteAsync(int optionId);
}