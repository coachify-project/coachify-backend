using Coachify.BLL.DTOs.Auth;
using Coachify.BLL.DTOs.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coachify.BLL.Interfaces
{
    public interface IUserService
    {
        Task<bool> RegisterAsync(CreateUserDto dto);
        Task<UserDto?> AuthenticateAsync(string email, string password);
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task<UserDto?> GetByIdAsync(int id);
        Task UpdateAsync(int id, UpdateUserDto dto);
        Task<bool> DeleteAsync(int id);
    }
}