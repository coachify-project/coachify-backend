using Microsoft.AspNetCore.Mvc;
using Coachify.BLL.DTOs.TestSubmission;
using Coachify.BLL.Interfaces;

namespace Coachify.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestSubmissionController : ControllerBase
{
    private readonly ITestSubmissionService _service;

    public TestSubmissionController(ITestSubmissionService service)
    {
        _service = service;
    }
    
    [HttpPost("submit")]
    public async Task<IActionResult> SubmitTest([FromBody] SubmitTestRequestDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return Ok(result);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var submissions = await _service.GetAllAsync();
        return Ok(submissions);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var submission = await _service.GetByIdAsync(id);
        if (submission == null)
            return NotFound();

        return Ok(submission);
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _service.DeleteAsync(id);
        return success ? Ok() : NotFound();
    }
}