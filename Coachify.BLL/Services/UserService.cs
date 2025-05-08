using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Coachify.BLL.DTOs.Auth;
using Coachify.BLL.DTOs.User;
using Coachify.BLL.Interfaces;
using Coachify.DAL;
using Coachify.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Coachify.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public UserService(ApplicationDbContext db, IMapper mapper, IConfiguration config)
        {
            _db = db;
            _mapper = mapper;
            _config = config;
        }

        // Получить всех пользователей
        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await _db.Users.ToListAsync();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        // Получить пользователя по ID
        public async Task<UserDto?> GetByIdAsync(int id)
        {
            var user = await _db.Users.FindAsync(id);
            return user == null ? null : _mapper.Map<UserDto>(user);
        }

        // Админская операция: создать пользователя
        public async Task<UserDto> CreateAsync(CreateUserDto dto)
        {
            var user = _mapper.Map<User>(dto);

            // Хешируем пароль перед сохранением
            using var hmac = new HMACSHA256();
            user.PasswordSalt = hmac.Key;
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password));

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return _mapper.Map<UserDto>(user);
        }

        // Админская операция: обновить пользователя
        public async Task UpdateAsync(int id, UpdateUserDto dto)
        {
            var user = await _db.Users.FindAsync(id);
            if (user == null) return;
            _mapper.Map(dto, user);
            await _db.SaveChangesAsync();
        }

        // Админская операция: удалить пользователя
        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _db.Users.FindAsync(id);
            if (user == null) return false;
            _db.Users.Remove(user);
            await _db.SaveChangesAsync();
            return true;
        }

        // Регистрация пользователя
        public async Task<bool> RegisterAsync(CreateUserDto dto)
        {
            // Проверка на существующий email
            var emailExists = await _db.Users.AnyAsync(u => u.Email == dto.Email);
            if (emailExists)
                return false; // Email уже существует

            var user = _mapper.Map<User>(dto);

            // Хешируем пароль перед сохранением
            using var hmac = new HMACSHA256();
            user.PasswordSalt = hmac.Key;
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password));

            // Присваиваем роль
            var role = await _db.Roles.FirstOrDefaultAsync(r => r.RoleName == "Client");
            if (role == null)
            {
                // Если роль "Client" не найдена, создаем её
                role = new Role { RoleName = "Client" };
                _db.Roles.Add(role);
                await _db.SaveChangesAsync();
            }

            user.RoleId = role.RoleId;

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return true; // Регистрация прошла успешно
        }

        // Вход: проверка и генерация JWT
        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _db.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null)
                throw new UnauthorizedAccessException("Неверный email или пароль");

            using var hmac = new HMACSHA256(user.PasswordSalt);
            var computedHash = hmac
                .ComputeHash(Encoding.UTF8.GetBytes(dto.Password));

            if (!computedHash.SequenceEqual(user.PasswordHash))
                throw new UnauthorizedAccessException("Неверный email или пароль");

            // Формирование JWT
            var jwtSection = _config.GetSection("Jwt");
            var keyBytes = Encoding.UTF8.GetBytes(jwtSection["Key"]!);
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Role, user.Role.RoleName)
            };

            var creds = new SigningCredentials(
                new SymmetricSecurityKey(keyBytes),
                SecurityAlgorithms.HmacSha256);

            var expires = DateTime.UtcNow
                          .AddMinutes(double.Parse(jwtSection["ExpireMinutes"]!));

            var token = new JwtSecurityToken(
                issuer: jwtSection["Issuer"],
                audience: jwtSection["Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return new AuthResponseDto
            {
                Token = jwt,
                ExpiresAt = expires
            };
        }
    }
}
