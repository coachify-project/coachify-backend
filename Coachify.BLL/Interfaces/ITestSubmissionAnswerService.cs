using Coachify.BLL.DTOs.TestSubmission;
using Coachify.BLL.DTOs.TestSubmissionAnswer;

namespace Coachify.BLL.Interfaces;

public interface ITestSubmissionAnswerService
{
    Task<IEnumerable<TestSubmissionAnswerDto>> GetAllAsync();
    Task<TestSubmissionAnswerDto?> GetByIdAsync(int id);
    Task<TestSubmissionAnswerDto> CreateAsync(CreateTestSubmissionAnswerDto dto);
    Task UpdateAsync(int id, UpdateTestSubmissionAnswerDto dto);
    Task<bool> DeleteAsync(int id);
}