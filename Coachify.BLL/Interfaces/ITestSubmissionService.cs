using Coachify.BLL.DTOs.TestSubmission;

namespace Coachify.BLL.Interfaces;

public interface ITestSubmissionService
{
    Task<IEnumerable<TestSubmissionDto>> GetAllAsync();
    Task<TestSubmissionDto?> GetByIdAsync(int id);
    Task<TestSubmissionDto> CreateAsync(CreateTestSubmissionDto dto);
    Task UpdateAsync(int id, UpdateTestSubmissionDto dto);
    Task<bool> DeleteAsync(int id);
}