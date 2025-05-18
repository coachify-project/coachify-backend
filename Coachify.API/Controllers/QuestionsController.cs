using Coachify.BLL.DTOs.Question;
using Coachify.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Coachify.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuestionsController : ControllerBase
{
    private readonly IQuestionService _service;
    public QuestionsController(IQuestionService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var q = await _service.GetByIdAsync(id);
        if (q == null) return NotFound();
        return Ok(q);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateQuestionDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.QuestionId }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateQuestionDto dto)
    {
        await _service.UpdateAsync(id, dto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id) => Ok(await _service.DeleteAsync(id));
}