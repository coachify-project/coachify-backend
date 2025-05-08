using Coachify.BLL.DTOs.Auth;
using Coachify.BLL.DTOs.User;

namespace Coachify.BLL.Interfaces
{
    public interface IUserService
    {
        Task<bool> RegisterAsync(CreateUserDto dto);
        Task<AuthResponseDto> LoginAsync(LoginDto dto);   

        Task<IEnumerable<UserDto>> GetAllAsync();
        Task<UserDto?> GetByIdAsync(int id);
        Task<UserDto> CreateAsync(CreateUserDto dto);
        Task UpdateAsync(int id, UpdateUserDto dto);
        Task<bool> DeleteAsync(int id);
        
    }
}