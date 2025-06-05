using Coachify.BLL.DTOs.Test;

namespace Coachify.BLL.Interfaces;

public interface ITestService
{
    Task<IEnumerable<TestDto>> GetAllAsync();
    Task<TestDto?> GetByIdAsync(int id);
    Task<TestDto> CreateAsync(CreateTestDto dto);
    Task<TestDto> CreateWithQuestionsAsync(CreateTestWithQuestionsDto dto);

    Task UpdateAsync(int id, UpdateTestDto dto);
    Task<bool> DeleteAsync(int id);
}