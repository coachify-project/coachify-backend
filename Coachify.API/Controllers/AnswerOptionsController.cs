using Coachify.BLL.DTOs.AnswerOption;
using Coachify.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Coachify.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnswerOptionsController : ControllerBase
{
    private readonly IAnswerOptionService _service;

    public AnswerOptionsController(IAnswerOptionService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var options = await _service.GetAllAsync();
        return Ok(options);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var option = await _service.GetByIdAsync(id);
        if (option == null)
            return NotFound();

        return Ok(option);
    }

    [HttpGet("question/{questionId}")]
    public async Task<IActionResult> GetByQuestionId(int questionId)
    {
        var options = await _service.GetByQuestionIdAsync(questionId);
        return Ok(options);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAnswerOptionDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.OptionId }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateAnswerOptionDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var updated = await _service.UpdateAsync(id, dto);
        if (updated == null)
            return NotFound();

        return Ok(updated); // Можно вернуть NoContent() если не требуется тело
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _service.DeleteAsync(id);
        if (!success)
            return NotFound();

        return NoContent();
    }
}