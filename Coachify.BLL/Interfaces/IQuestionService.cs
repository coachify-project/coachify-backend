using Coachify.BLL.DTOs.Question;

namespace Coachify.BLL.Interfaces;

public interface IQuestionService
{
    Task<IEnumerable<QuestionDto>> GetAllAsync();

    Task<QuestionDto?> GetByIdAsync(int questionId);
    Task<IEnumerable<QuestionDto>> GetByTestIdAsync(int testId);
    Task<QuestionDto> CreateAsync(CreateQuestionDto dto);
    Task<QuestionDto?> UpdateAsync(int questionId, UpdateQuestionDto dto);
    Task<bool> DeleteAsync(int questionId);
}