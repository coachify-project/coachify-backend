using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Coachify.API.Settings;
using Coachify.BLL.DTOs.Auth;
using Coachify.BLL.DTOs.User;
using Coachify.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Coachify.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly JwtSettings _jwtSettings;

        public UsersController(IUserService service, IOptions<JwtSettings> jwtOptions)
        {
            _service = service;
            _jwtSettings = jwtOptions.Value;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateUserDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = await _service.RegisterAsync(dto);
            if (!success)
                return Conflict("Email уже зарегистрирован.");

            return Ok("Зарегистрировано");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest("Email и пароль обязательны.");

            var user = await _service.AuthenticateAsync(dto.Email, dto.Password);
            if (user == null)
                return Unauthorized("Неверный email или пароль.");

            var roleName = user.RoleId switch
            {
                1 => "Admin",
                2 => "Coach",
                3 => "Client",
                _ => "User"
            };

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Role, roleName)
            };

            var key = Encoding.ASCII.GetBytes(_jwtSettings.Key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.TokenLifetimeMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new
            {
                Token = tokenHandler.WriteToken(token),
                User = user
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            return dto != null ? Ok(dto) : NotFound();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUserDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _service.GetByIdAsync(id);
            if (user == null)
                return NotFound();

            await _service.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }

        // [Authorize]
        // [HttpGet("profile")]
        // public async Task<IActionResult> Profile()
        // {
        //     var userIdClaim = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        //     if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
        //         return Unauthorized("Пользователь не авторизован.");
        //
        //     var user = await _service.GetByIdAsync(userId);
        //     return Ok(user);
        // }
    }
}
