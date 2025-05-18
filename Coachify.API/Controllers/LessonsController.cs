using Coachify.BLL.DTOs.Lesson;
using Coachify.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Coachify.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LessonsController : ControllerBase
{
    private readonly ILessonService _service;
    public LessonsController(ILessonService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var d = await _service.GetByIdAsync(id);
        return d == null ? NotFound() : Ok(d);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateLessonDto dto)
    {
        var createdLesson = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(Get), new { id = createdLesson.LessonId }, createdLesson);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateLessonDto dto)
    {
        await _service.UpdateAsync(id, dto);
        return NoContent();
    }
    
    // LessonsController.cs
    [HttpPost("start/{userId}/{lessonId}")]
    public async Task<ActionResult> StartLesson(int userId, int lessonId)
    {
        try
        {
            var result = await _service.StartLessonAsync(userId, lessonId);
            return Ok(new { success = result });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "Произошла ошибка при начале прохождения урока");
        }
    }

    [HttpPost("complete/{userId}/{lessonId}")]
    public async Task<ActionResult> CompleteLesson(int userId, int lessonId)
    {
        try
        {
            var result = await _service.CompleteLessonAsync(userId, lessonId);
            return Ok(new { success = result });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "Произошла ошибка при завершении урока");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id) => Ok(await _service.DeleteAsync(id));
}

