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
        try
        {
            if (dto == null)
            {
                Console.WriteLine("Request data is null");
                return BadRequest("Request data is null");
            }
        
            if (dto.Answers == null || !dto.Answers.Any())
            {
                Console.WriteLine("No answers provided");
                return BadRequest("No answers provided");
            }
        
            Console.WriteLine($"Received TestId: {dto.TestId}, UserId: {dto.UserId}");
            Console.WriteLine($"Answers count: {dto.Answers.Count}");
        
            var result = await _service.CreateAsync(dto);
        
            Console.WriteLine($"Service returned result: {result != null}");
            if (result != null)
            {
                Console.WriteLine($"Result Score: {result.Score}");
                Console.WriteLine($"Result IsPassed: {result.IsPassed}");
            }
        
            // Create a safe response object to avoid serialization conflicts
            var safeResponse = new
            {
                score = result?.Score ?? 0,
                isPassed = result?.IsPassed ?? false,
                correctAnswers = result?.CorrectAnswers ?? 0,
                submittedAt = result?.SubmittedAt ?? DateTime.Now,
                userId =  dto.UserId,
                testId =  dto.TestId
            };
        
            return Ok(safeResponse);
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("JSON property name") && ex.Message.Contains("collides"))
        {
            Console.WriteLine($"JSON serialization conflict: {ex.Message}");
            return StatusCode(500, new { error = "Internal serialization error. Please contact support." });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in SubmitTest: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            return BadRequest($"Error: {ex.Message}");
        }
    }
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var submissions = await _service.GetAllAsync();
            return Ok(submissions);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetAll: {ex.Message}");
            return BadRequest($"Error: {ex.Message}");
        }
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var submission = await _service.GetByIdAsync(id);
            if (submission == null)
                return NotFound();

            return Ok(submission);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetById: {ex.Message}");
            return BadRequest($"Error: {ex.Message}");
        }
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var success = await _service.DeleteAsync(id);
            return success ? Ok() : NotFound();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in Delete: {ex.Message}");
            return BadRequest($"Error: {ex.Message}");
        }
    }

    // Дополнительный метод для получения результатов пользователя по тесту
    [HttpGet("user/{userId}/test/{testId}")]
    public async Task<IActionResult> GetUserTestResult(int userId, int testId)
    {
        try
        {
            var submissions = await _service.GetAllAsync();
            var userSubmission = submissions
                .Where(s => s.UserId == userId && s.TestId == testId)
                .OrderByDescending(s => s.SubmittedAt)
                .FirstOrDefault();

            if (userSubmission == null)
                return NotFound("No submission found for this user and test");

            return Ok(userSubmission);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetUserTestResult: {ex.Message}");
            return BadRequest($"Error: {ex.Message}");
        }
    }

    // Метод для получения всех результатов пользователя
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserResults(int userId)
    {
        try
        {
            var submissions = await _service.GetAllAsync();
            var userSubmissions = submissions.Where(s => s.UserId == userId).ToList();

            return Ok(userSubmissions);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetUserResults: {ex.Message}");
            return BadRequest($"Error: {ex.Message}");
        }
    }
}