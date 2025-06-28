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
                Console.WriteLine($"Correct Answers: {result.correctAnswers}/{result.totalQuestions}");
            }
        
            // Создаем полный ответ с всеми необходимыми данными для фронтенда
            var response = new
            {
                // Основные данные
                score = result?.Score ?? 0,                           // Окончательный балл в процентах
                scorePercentage = result?.scorePercentage ?? 0,       // Дублируем для совместимости
                isPassed = result?.IsPassed ?? false,                 // Прошел ли тест
                passed = result?.passed ?? false,                     // Дублируем для совместимости
                
                // Детальная статистика
                correctAnswers = result?.correctAnswers ?? 0,         // Количество правильных ответов
                totalQuestions = result?.totalQuestions ?? 0,         // Общее количество вопросов
                passingScore = result?.passingScore ?? 70,            // Проходной балл
                
                // Метаданные
                submittedAt = result?.SubmittedAt ?? DateTime.Now,    // Время сдачи
                userId = dto.UserId,                                  // ID пользователя
                testId = dto.TestId,                                  // ID теста
                
                // Дополнительная информация для UI
                message = result?.IsPassed == true ? "Тест успешно пройден!" : "Тест не пройден. Попробуйте еще раз.",
                percentage = $"{result?.Score ?? 0}%"                 // Форматированный процент
            };
        
            return Ok(response);
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
            return BadRequest(new { error = ex.Message, details = "Произошла ошибка при обработке теста" });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var submissions = await _service.GetAllAsync();
            
            // Преобразуем данные для удобного отображения на фронтенде
            var formattedSubmissions = submissions.Select(s => new
            {
                submissionId = s.SubmissionId,
                testId = s.TestId,
                userId = s.UserId,
                score = s.Score,
                scorePercentage = $"{s.Score}%",
                isPassed = s.IsPassed,
                correctAnswers = s.CorrectAnswers,
                totalQuestions = s.TotalQuestions,
                submittedAt = s.SubmittedAt,
                status = s.IsPassed ? "Пройден" : "Не пройден"
            }).ToList();

            return Ok(formattedSubmissions);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetAll: {ex.Message}");
            return BadRequest(new { error = ex.Message });
        }
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var submission = await _service.GetByIdAsync(id);
            if (submission == null)
                return NotFound(new { message = "Результат теста не найден" });

            // Форматируем данные для фронтенда
            var formattedSubmission = new
            {
                submissionId = submission.SubmissionId,
                testId = submission.TestId,
                userId = submission.UserId,
                score = submission.Score,
                scorePercentage = $"{submission.Score}%",
                isPassed = submission.IsPassed,
                correctAnswers = submission.CorrectAnswers,
                totalQuestions = submission.TotalQuestions,
                submittedAt = submission.SubmittedAt,
                status = submission.IsPassed ? "Пройден" : "Не пройден",
                
                // Дополнительная статистика
                incorrectAnswers = submission.TotalQuestions - submission.CorrectAnswers,
                accuracyRate = submission.TotalQuestions > 0 
                    ? Math.Round((double)submission.CorrectAnswers / submission.TotalQuestions * 100, 1) 
                    : 0
            };

            return Ok(formattedSubmission);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetById: {ex.Message}");
            return BadRequest(new { error = ex.Message });
        }
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var success = await _service.DeleteAsync(id);
            if (success)
            {
                return Ok(new { message = "Результат теста успешно удален" });
            }
            return NotFound(new { message = "Результат теста не найден" });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in Delete: {ex.Message}");
            return BadRequest(new { error = ex.Message });
        }
    }

    // Получение результатов пользователя по конкретному тесту (последняя попытка)
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
                return NotFound(new { message = "Результаты теста для данного пользователя не найдены" });

            var formattedResult = new
            {
                submissionId = userSubmission.SubmissionId,
                testId = userSubmission.TestId,
                userId = userSubmission.UserId,
                score = userSubmission.Score,
                scorePercentage = $"{userSubmission.Score}%",
                isPassed = userSubmission.IsPassed,
                correctAnswers = userSubmission.CorrectAnswers,
                totalQuestions = userSubmission.TotalQuestions,
                submittedAt = userSubmission.SubmittedAt,
                status = userSubmission.IsPassed ? "Пройден" : "Не пройден",
                
                // Дополнительная статистика
                incorrectAnswers = userSubmission.TotalQuestions - userSubmission.CorrectAnswers,
                accuracyRate = userSubmission.TotalQuestions > 0 
                    ? Math.Round((double)userSubmission.CorrectAnswers / userSubmission.TotalQuestions * 100, 1) 
                    : 0,
                attempts = submissions.Count(s => s.UserId == userId && s.TestId == testId) // Количество попыток
            };

            return Ok(formattedResult);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetUserTestResult: {ex.Message}");
            return BadRequest(new { error = ex.Message });
        }
    }

    // Получение всех результатов пользователя с группировкой по тестам
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserResults(int userId)
    {
        try
        {
            var submissions = await _service.GetAllAsync();
            var userSubmissions = submissions.Where(s => s.UserId == userId).ToList();

            if (!userSubmissions.Any())
                return Ok(new { message = "Пользователь еще не проходил тесты", results = new List<object>() });

            // Группируем по тестам и берем лучший результат для каждого теста
            var groupedResults = userSubmissions
                .GroupBy(s => s.TestId)
                .Select(group => new
                {
                    testId = group.Key,
                    totalAttempts = group.Count(),
                    bestResult = group.OrderByDescending(s => s.Score).First(),
                    lastAttempt = group.OrderByDescending(s => s.SubmittedAt).First(),
                    allAttempts = group.OrderByDescending(s => s.SubmittedAt).Select(s => new
                    {
                        submissionId = s.SubmissionId,
                        score = s.Score,
                        scorePercentage = $"{s.Score}%",
                        isPassed = s.IsPassed,
                        correctAnswers = s.CorrectAnswers,
                        totalQuestions = s.TotalQuestions,
                        submittedAt = s.SubmittedAt
                    }).ToList()
                })
                .Select(g => new
                {
                    g.testId,
                    g.totalAttempts,
                    bestScore = g.bestResult.Score,
                    bestScorePercentage = $"{g.bestResult.Score}%",
                    isPassed = g.bestResult.IsPassed,
                    lastAttemptDate = g.lastAttempt.SubmittedAt,
                    g.allAttempts
                })
                .ToList();

            var summary = new
            {
                userId = userId,
                totalTestsTaken = groupedResults.Count,
                totalTestsPassed = groupedResults.Count(r => r.isPassed),
                averageScore = groupedResults.Any() 
                    ? Math.Round(groupedResults.Average(r => r.bestScore), 1) 
                    : 0,
                results = groupedResults
            };

            return Ok(summary);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetUserResults: {ex.Message}");
            return BadRequest(new { error = ex.Message });
        }
    }

    // Дополнительный метод для получения статистики по тесту
    [HttpGet("test/{testId}/statistics")]
    public async Task<IActionResult> GetTestStatistics(int testId)
    {
        try
        {
            var submissions = await _service.GetAllAsync();
            var testSubmissions = submissions.Where(s => s.TestId == testId).ToList();

            if (!testSubmissions.Any())
                return Ok(new { message = "По данному тесту еще нет результатов", statistics = new object() });

            var statistics = new
            {
                testId = testId,
                totalSubmissions = testSubmissions.Count,
                uniqueUsers = testSubmissions.Select(s => s.UserId).Distinct().Count(),
                passedSubmissions = testSubmissions.Count(s => s.IsPassed),
                failedSubmissions = testSubmissions.Count(s => !s.IsPassed),
                passRate = Math.Round((double)testSubmissions.Count(s => s.IsPassed) / testSubmissions.Count * 100, 1),
                averageScore = Math.Round(testSubmissions.Average(s => s.Score), 1),
                highestScore = testSubmissions.Max(s => s.Score),
                lowestScore = testSubmissions.Min(s => s.Score),
                averageCorrectAnswers = Math.Round(testSubmissions.Average(s => s.CorrectAnswers), 1),
                totalQuestions = testSubmissions.FirstOrDefault()?.TotalQuestions ?? 0
            };

            return Ok(statistics);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetTestStatistics: {ex.Message}");
            return BadRequest(new { error = ex.Message });
        }
    }
}