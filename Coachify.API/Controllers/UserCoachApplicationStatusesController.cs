using Coachify.BLL.DTOs.UserCoachApplicationStatus;
using Coachify.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class UserCoachApplicationStatusesController : ControllerBase
{
    private readonly IUserCoachApplicationStatusService _service;
    public UserCoachApplicationStatusesController(IUserCoachApplicationStatusService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var d = await _service.GetByIdAsync(id);
        return d == null ? NotFound() : Ok(d);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateUserCoachApplicationStatusDto dto)
    {
        var c = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(Get), new { id = c.StatusId }, c);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateUserCoachApplicationStatusDto dto)
    {
        await _service.UpdateAsync(id, dto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id) => Ok(await _service.DeleteAsync(id));
}