using Coachify.BLL.DTOs.CourseStatus;
using Coachify.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class CourseStatusesController : ControllerBase
{
    private readonly ICourseStatusService _service;
    public CourseStatusesController(ICourseStatusService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var d = await _service.GetByIdAsync(id);
        return d == null ? NotFound() : Ok(d);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCourseStatusDto dto)
    {
        var c = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(Get), new { id = c.StatusId }, c);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateCourseStatusDto dto)
    {
        await _service.UpdateAsync(id, dto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id) => Ok(await _service.DeleteAsync(id));
}