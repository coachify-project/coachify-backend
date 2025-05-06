using Coachify.BLL.DTOs.Question;

namespace Coachify.BLL.Interfaces;

public interface IQuestionService
{
    Task<IEnumerable<QuestionDto>> GetAllAsync();
    Task<QuestionDto?> GetByIdAsync(int id);
    Task<QuestionDto> CreateAsync(CreateQuestionDto dto);
    Task UpdateAsync(int id, UpdateQuestionDto dto);
    Task<bool> DeleteAsync(int id);
}