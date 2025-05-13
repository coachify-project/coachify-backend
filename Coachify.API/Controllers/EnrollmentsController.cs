using Coachify.BLL.DTOs.Enrollment;
using Coachify.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Coachify.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EnrollmentsController : ControllerBase
{
    private readonly IEnrollmentService _service;
    public EnrollmentsController(IEnrollmentService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var d = await _service.GetByIdAsync(id);
        return d == null ? NotFound() : Ok(d);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateEnrollmentDto dto)
    {
        var c = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(Get), new { id = c.Id }, c);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateEnrollmentDto dto)
    {
        await _service.UpdateAsync(id, dto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id) => Ok(await _service.DeleteAsync(id));
    
    [HttpPost("{courseId}/enroll")]
    public async Task<IActionResult> EnrollUser(int courseId, [FromQuery] int userId)
    {
        var success = await _service.EnrollUserAsync(courseId, userId);
        return success ? Ok() : BadRequest("User is already enrolled.");
    }

    [HttpPost("{courseId}/start")]
    public async Task<IActionResult> StartCourse(int courseId, [FromQuery] int userId)
    {
        var success = await _service.StartCourseAsync(courseId, userId);
        return success ? Ok() : BadRequest("Cannot start course.");
    }

    [HttpPost("{courseId}/complete")]
    public async Task<IActionResult> CompleteCourse(int courseId, [FromQuery] int userId)
    {
        var success = await _service.CompleteCourseAsync(courseId, userId);
        return success ? Ok() : BadRequest("Cannot complete course.");
    }
}