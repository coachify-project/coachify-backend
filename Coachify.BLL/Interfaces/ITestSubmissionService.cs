using Coachify.BLL.DTOs.TestSubmission;

namespace Coachify.BLL.Interfaces;
public interface ITestSubmissionService
{
    Task<TestSubmissionResultDto> CreateAsync(SubmitTestRequestDto dto);
    Task<IEnumerable<TestSubmissionDto>> GetAllAsync();
    Task<TestSubmissionDto?> GetByIdAsync(int id);
    Task<bool> DeleteAsync(int id);
}
