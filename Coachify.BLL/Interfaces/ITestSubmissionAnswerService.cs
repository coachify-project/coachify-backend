using Coachify.BLL.DTOs.TestSubmission;
using Coachify.BLL.DTOs.TestSubmissionAnswer;

namespace Coachify.BLL.Interfaces;

public interface ITestSubmissionAnswerService
{
    Task<IEnumerable<TestSubmissionAnswerDto>> GetAllAsync();
    Task<TestSubmissionAnswerDto?> GetByIdAsync(int id);
    Task<bool> DeleteAsync(int id);
}