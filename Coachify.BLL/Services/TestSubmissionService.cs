using AutoMapper;
using Coachify.BLL.DTOs.TestSubmission;
using Coachify.DAL;
using Coachify.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Coachify.BLL.Interfaces;

public class TestSubmissionService : ITestSubmissionService
{
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;
    private readonly IEnrollmentService _enrollmentService;
    private readonly IModuleService _moduleService;
    private readonly IProgressService _progressService;

    public TestSubmissionService(ApplicationDbContext db, IMapper mapper, IEnrollmentService enrollmentService,
        IModuleService moduleService,IProgressService progressService)
    {
        _db = db;
        _mapper = mapper;
        _enrollmentService = enrollmentService;
        _moduleService = moduleService;
        _progressService = progressService;
    }

    public async Task<IEnumerable<TestSubmissionDto>> GetAllAsync()
    {
        var submissions = await _db.TestSubmissions
            .Include(ts => ts.Test)
            .AsNoTracking()
            .ToListAsync();

        var results = new List<TestSubmissionDto>();

        foreach (var s in submissions)
        {
            int totalQuestions = await _db.Questions.CountAsync(q => q.TestId == s.TestId);
            int correctAnswers = await _db.TestSubmissionAnswers
                .Where(a => a.SubmissionId == s.SubmissionId && a.Option.IsCorrect)
                .CountAsync();

            var dto = _mapper.Map<TestSubmissionDto>(s);
            dto.TotalQuestions = totalQuestions;
            dto.CorrectAnswers = correctAnswers;
            results.Add(dto);
        }

        return results;
    }

    public async Task<TestSubmissionDto?> GetByIdAsync(int id)
    {
        var submission = await _db.TestSubmissions
            .Include(ts => ts.Test)
            .AsNoTracking()
            .FirstOrDefaultAsync(ts => ts.SubmissionId == id);

        if (submission == null)
            return null;

        int totalQuestions = await _db.Questions.CountAsync(q => q.TestId == submission.TestId);
        int correctAnswers = await _db.TestSubmissionAnswers
            .Where(a => a.SubmissionId == id && a.Option.IsCorrect)
            .CountAsync();

        var dto = _mapper.Map<TestSubmissionDto>(submission);
        dto.TotalQuestions = totalQuestions;
        dto.CorrectAnswers = correctAnswers;
        return dto;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var submission = await _db.TestSubmissions.FindAsync(id);
        if (submission == null)
            return false;

        // Удаляем связанные ответы сначала
        var answers = _db.TestSubmissionAnswers.Where(a => a.SubmissionId == id);
        _db.TestSubmissionAnswers.RemoveRange(answers);

        _db.TestSubmissions.Remove(submission);
        await _db.SaveChangesAsync();
        return true;
    }

   public async Task<TestSubmissionResultDto> CreateAsync(SubmitTestRequestDto dto)
{
    try
    {
        Console.WriteLine($"Starting CreateAsync with TestId: {dto.TestId}, UserId: {dto.UserId}");
        
        // Проверки на null
        if (dto == null)
            throw new ArgumentNullException(nameof(dto));
        
        if (dto.Answers == null)
            throw new ArgumentNullException(nameof(dto.Answers));
        
        // 1. Загрузка теста с информацией о проходном балле
        Console.WriteLine("Loading test and questions...");
        var test = await _db.Tests
            .Include(t => t.Module)
            .FirstOrDefaultAsync(t => t.TestId == dto.TestId);
            
        if (test == null)
            throw new KeyNotFoundException($"Test with ID {dto.TestId} not found");
        
        var questions = await _db.Questions
            .Where(q => q.TestId == dto.TestId)
            .Include(q => q.Options)
            .ToListAsync();

        Console.WriteLine($"Found {questions.Count} questions");
        
        if (!questions.Any())
            throw new KeyNotFoundException($"Test with ID {dto.TestId} has no questions");

        // 2. Словарь всех опций по ID
        var optionMap = questions.SelectMany(q => q.Options).ToDictionary(o => o.OptionId);
        Console.WriteLine($"Created option map with {optionMap.Count} options");

        // 3. Создание сабмишена
        Console.WriteLine("Creating submission...");
        var submission = new TestSubmission
        {
            TestId = dto.TestId,
            UserId = dto.UserId,
            SubmittedAt = DateTime.UtcNow
        };

        _db.TestSubmissions.Add(submission);
        await _db.SaveChangesAsync();
        Console.WriteLine($"Created submission with ID: {submission.SubmissionId}");

        int correctAnswers = 0;
        var submissionAnswers = new List<TestSubmissionAnswer>();

        // 4. Обработка всех ответов пользователя
        Console.WriteLine("Processing answers...");
        foreach (var answer in dto.Answers)
        {
            Console.WriteLine($"Processing question {answer.QuestionId} with {answer.SelectedOptionIds.Count} options");
            
            if (answer.SelectedOptionIds == null)
            {
                Console.WriteLine($"SelectedOptionIds is null for question {answer.QuestionId}");
                continue;
            }
            
            foreach (var optionId in answer.SelectedOptionIds.Distinct())
            {
                if (optionMap.TryGetValue(optionId, out var option) &&
                    option.QuestionId == answer.QuestionId)
                {
                    if (option.IsCorrect) 
                    {
                        correctAnswers++;
                        Console.WriteLine($"Correct answer found: Option {optionId}");
                    }

                    submissionAnswers.Add(new TestSubmissionAnswer
                    {
                        SubmissionId = submission.SubmissionId,
                        QuestionId = answer.QuestionId,
                        OptionId = optionId
                    });
                }
                else
                {
                    Console.WriteLine($"Option {optionId} not found or doesn't belong to question {answer.QuestionId}");
                }
            }
        }

        // Добавляем все ответы
        if (submissionAnswers.Any())
        {
            _db.TestSubmissionAnswers.AddRange(submissionAnswers);
            Console.WriteLine($"Adding {submissionAnswers.Count} submission answers");
        }

        int totalQuestions = questions.Count;
        int scorePercentage = totalQuestions > 0
            ? (int)Math.Round(100.0 * correctAnswers / totalQuestions)
            : 0;

        // Получаем проходной балл из теста (если есть) или используем дефолтный
        int passingScore = test.PassScore ; // Дефолтный проходной балл 70%
        
        submission.Score = scorePercentage;
        submission.IsPassed = scorePercentage >= passingScore;
        
        Console.WriteLine($"Score: {scorePercentage}%, Correct: {correctAnswers}/{totalQuestions}, Passed: {submission.IsPassed}, PassingScore: {passingScore}%");
        
        await _db.SaveChangesAsync();

        // 5. Проверка завершения курса и модуля
        Console.WriteLine("Checking course/module completion...");
        if (test?.Module != null)
        {
            Console.WriteLine($"Test found, ModuleId: {test.ModuleId}");
            var courseId = test.Module.CourseId;

            // Логика завершения курса
            var enrollment = await _db.Enrollments
                .Include(e => e.Course)
                .ThenInclude(c => c.Modules)
                .ThenInclude(m => m.Test)
                .FirstOrDefaultAsync(e =>
                    e.CourseId == courseId &&
                    e.UserId == dto.UserId);

            if (enrollment != null && submission.IsPassed)
            {
                Console.WriteLine("Checking if all course tests are passed...");
                bool allPassed = enrollment.Course.Modules.All(m =>
                    m.Test == null ||
                    _db.TestSubmissions.Any(ts =>
                        ts.TestId == m.Test.TestId &&
                        ts.UserId == dto.UserId &&
                        ts.IsPassed));

                if (allPassed && enrollment.StatusId != 3)
                {
                    Console.WriteLine("Completing enrollment...");
                    if (_enrollmentService != null)
                        await _enrollmentService.CompleteEnrollmentAsync(enrollment.EnrollmentId);
                }
            }

            // Завершение модуля
            if (submission.IsPassed)
            {
                Console.WriteLine("Checking module completion...");
                bool allModuleTestsPassed = await _db.TestSubmissions
                    .Where(ts => ts.UserId == dto.UserId && ts.Test.ModuleId == test.ModuleId)
                    .AllAsync(ts => ts.IsPassed);

                if (allModuleTestsPassed)
                {
                    Console.WriteLine("Completing module...");
                    if (_progressService != null)
                        await _progressService.CompleteModuleAsync(dto.UserId, test.ModuleId);
                }
            }
        }
        else
        {
            Console.WriteLine("Test or Module not found");
        }

        Console.WriteLine("CreateAsync completed successfully");
        
        // ИСПРАВЛЕННЫЙ ВОЗВРАТ ДАННЫХ - используем правильные имена полей
        return new TestSubmissionResultDto
        {
            Score = scorePercentage, // Основной балл
            scorePercentage = scorePercentage, // Для совместимости с фронтендом
            correctAnswers = correctAnswers, // Lowercase для фронтенда
            totalQuestions = totalQuestions, // Lowercase для фронтенда
            passingScore = passingScore, // Проходной балл
            passed = submission.IsPassed, // Статус прохождения
            IsPassed = submission.IsPassed, // Для совместимости
            SubmittedAt = submission.SubmittedAt
        };
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error in CreateAsync: {ex.Message}");
        Console.WriteLine($"Stack trace: {ex.StackTrace}");
        throw;
    }
}
}