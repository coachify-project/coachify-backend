using Microsoft.AspNetCore.Mvc;
using Coachify.BLL.Interfaces;
using Coachify.BLL.DTOs.TestSubmissionAnswer;

namespace Coachify.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestSubmissionAnswerController : ControllerBase
{
    private readonly ITestSubmissionAnswerService _service;

    public TestSubmissionAnswerController(ITestSubmissionAnswerService service)
    {
        _service = service;
    }

    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var answers = await _service.GetAllAsync();
        return Ok(answers);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var answer = await _service.GetByIdAsync(id);
        if (answer == null)
            return NotFound();

        return Ok(answer);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _service.DeleteAsync(id);
        return success ? Ok() : NotFound();
    }
}