using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Coachify.BLL.DTOs.Auth;
using Coachify.BLL.DTOs.User;
using Coachify.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coachify.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _service;

    public UsersController(IUserService service) =>
        _service = service;

    [HttpPost("register")]
    public async Task<IActionResult> Register(CreateUserDto dto)
    {
        var success = await _service.RegisterAsync(dto);
        if (!success)
            return Conflict("Email уже зарегистрирован.");
    
        return Ok("Зарегистрировано");
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
        return deleted ?NoContent() : NotFound();
    }
    

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        try
        {
            var result = await _service.LoginAsync(dto);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ex.Message);
        }
    }

    // Пример защищённого эндпоинта
    [Authorize]
    [HttpGet("profile")]
    public async Task<IActionResult> Profile()
    {
        var userId = int.Parse(User.FindFirstValue(JwtRegisteredClaimNames.Sub)!);
        var user   = await _service.GetByIdAsync(userId);
        return Ok(user);
    }
}
