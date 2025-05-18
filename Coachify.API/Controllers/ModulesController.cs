using Coachify.BLL.DTOs.Module;
using Coachify.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Coachify.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ModulesController : ControllerBase
{
    private readonly IModuleService _service;
    public ModulesController(IModuleService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var list = await _service.GetAllAsync();
        return Ok(list);
    }

    [HttpGet("course/{courseId}")]
    public async Task<IActionResult> GetByCourse(int courseId)
    {
        var list = await _service.GetAllByCourseAsync(courseId);
        return Ok(list);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var module = await _service.GetByIdAsync(id);
        return module == null ? NotFound() : Ok(module);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateModuleDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(Get), new { id = created.ModuleId }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateModuleDto dto)
    {
        try
        {
            var updated = await _service.UpdateAsync(id, dto);
            return Ok(updated);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }


    // ModulesController.cs
    [HttpPost("{moduleId}/start")]
    public async Task<ActionResult> StartModule(int moduleId, [FromQuery] int userId)
    {
        try
        {
            var result = await _service.StartModuleAsync(userId, moduleId);
            return Ok(new { success = result });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "Произошла ошибка при начале прохождения модуля");
        }
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id) => Ok(await _service.DeleteAsync(id));
}