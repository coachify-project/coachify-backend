using Coachify.BLL.DTOs.AnswerOption;
using Coachify.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Coachify.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnswerOptionsController : ControllerBase
{
    private readonly IAnswerOptionService _service;
    public AnswerOptionsController(IAnswerOptionService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var opt = await _service.GetByIdAsync(id);
        if (opt == null) return NotFound();
        return Ok(opt);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAnswerOptionDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.OptionId }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateAnswerOptionDto dto)
    {
        await _service.UpdateAsync(id, dto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id) => Ok(await _service.DeleteAsync(id));
}