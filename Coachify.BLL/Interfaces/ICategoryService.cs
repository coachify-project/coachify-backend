using Coachify.BLL.DTOs.Categoty;

namespace Coachify.BLL.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<CategoryDto>> GetAllAsync();
    Task<CategoryDto?> GetByIdAsync(int id);
    Task<CategoryDto> CreateAsync(CreateCategoryDto dto);
    Task UpdateAsync(int id, UpdateCategoryDto dto);
    Task<bool> DeleteAsync(int id);
}