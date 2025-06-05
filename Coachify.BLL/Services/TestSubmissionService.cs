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
        IModuleService moduleService)
    {
        _db = db;
        _mapper = mapper;
        _enrollmentService = enrollmentService;
        _moduleService = moduleService;
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
        // 1. Загрузка всех вопросов и опций по тесту
        var questions = await _db.Questions
            .Where(q => q.TestId == dto.TestId)
            .Include(q => q.Options)
            .ToListAsync();

        if (!questions.Any())
            throw new KeyNotFoundException("Test or questions not found");

        // 2. Словарь всех опций по ID
        var optionMap = questions.SelectMany(q => q.Options).ToDictionary(o => o.OptionId);

        // 3. Создание сабмишена
        var submission = new TestSubmission
        {
            TestId = dto.TestId,
            UserId = dto.UserId,
            SubmittedAt = DateTime.UtcNow
        };

        _db.TestSubmissions.Add(submission);
        await _db.SaveChangesAsync();

        int correctAnswers = 0;

        // 4. Обработка всех ответов пользователя
        foreach (var answer in dto.Answers)
        {
            foreach (var optionId in answer.SelectedOptionIds.Distinct())
            {
                if (optionMap.TryGetValue(optionId, out var option) &&
                    option.QuestionId == answer.QuestionId)
                {
                    if (option.IsCorrect) correctAnswers++;

                    _db.TestSubmissionAnswers.Add(new TestSubmissionAnswer
                    {
                        SubmissionId = submission.SubmissionId,
                        QuestionId = answer.QuestionId,
                        OptionId = optionId
                    });
                }
            }
        }

        int totalQuestions = questions.Count;

        submission.Score = totalQuestions > 0
            ? (int)Math.Round(100.0 * correctAnswers / totalQuestions)
            : 0;

        submission.IsPassed = submission.Score >= 30;
        await _db.SaveChangesAsync();

        // 5. Проверка завершения курса
        // Загружаем тест вместе с модулем
        var test = await _db.Tests
            .Include(t => t.Module)
            .FirstOrDefaultAsync(t => t.TestId == submission.TestId);

        if (test == null || test.Module == null)
            throw new InvalidOperationException("Test or module not found");

// Теперь можем безопасно получить CourseId
        var courseId = test.Module.CourseId;

        var enrollment = await _db.Enrollments
            .Include(e => e.Course)
            .ThenInclude(c => c.Modules)
            .ThenInclude(m => m.Test)
            .FirstOrDefaultAsync(e =>
                e.CourseId == courseId &&
                e.UserId == dto.UserId);


        if (enrollment != null && submission.IsPassed)
        {
            bool allPassed = enrollment.Course.Modules.All(m =>
                m.Test == null ||
                _db.TestSubmissions.Any(ts =>
                    ts.TestId == m.Test.TestId &&
                    ts.UserId == dto.UserId &&
                    ts.IsPassed));

            if (allPassed && enrollment.StatusId != 3)
            {
                await _enrollmentService.CompleteEnrollmentAsync(enrollment.EnrollmentId);
            }
        }

        // --- Новый код: завершение модуля ---

        // Получаем модуль по тесту
        var module = await _db.Modules
            .FirstOrDefaultAsync(m => m.ModuleId == submission.Test.ModuleId);

        if (module != null)
        {
            // Проверяем, все ли тесты модуля пройдены пользователем
            bool allModuleTestsPassed = await _db.TestSubmissions
                .Where(ts => ts.UserId == dto.UserId && ts.Test.ModuleId == module.ModuleId)
                .AllAsync(ts => ts.IsPassed);

            if (allModuleTestsPassed)
            {
                // Вызываем метод завершения модуля
                await _progressService.CompleteModuleAsync(dto.UserId, module.ModuleId);
            }
        }

        return new TestSubmissionResultDto
        {
            Score = submission.Score,
            CorrectAnswers = correctAnswers,
            TotalQuestions = totalQuestions
        };
    }
}