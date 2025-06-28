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
            int correctAnswers = await CalculateCorrectAnswersCount(s.SubmissionId, s.TestId);

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
        int correctAnswers = await CalculateCorrectAnswersCount(id, submission.TestId);

        var dto = _mapper.Map<TestSubmissionDto>(submission);
        dto.TotalQuestions = totalQuestions;
        dto.CorrectAnswers = correctAnswers;
        return dto;
    }

    // Вспомогательный метод для подсчета правильных ответов
    private async Task<int> CalculateCorrectAnswersCount(int submissionId, int testId)
    {
        // Получаем все вопросы теста с их опциями
        var questions = await _db.Questions
            .Where(q => q.TestId == testId)
            .Include(q => q.Options)
            .ToListAsync();

        // Получаем все ответы пользователя для этого submission
        var userAnswers = await _db.TestSubmissionAnswers
            .Where(a => a.SubmissionId == submissionId)
            .ToListAsync();

        int correctAnswersCount = 0;

        foreach (var question in questions)
        {
            // Получаем правильные опции для вопроса
            var correctOptionIds = question.Options
                .Where(o => o.IsCorrect)
                .Select(o => o.OptionId)
                .ToHashSet();

            // Получаем выбранные пользователем опции для этого вопроса
            var selectedOptionIds = userAnswers
                .Where(ua => ua.QuestionId == question.QuestionId)
                .Select(ua => ua.OptionId)
                .ToHashSet();

            // Проверяем, правильно ли ответил пользователь
            if (correctOptionIds.SetEquals(selectedOptionIds))
            {
                correctAnswersCount++;
            }
        }

        return correctAnswersCount;
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

            int correctAnswersCount = 0; // Количество правильно отвеченных вопросов
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

                // Находим вопрос
                var question = questions.FirstOrDefault(q => q.QuestionId == answer.QuestionId);
                if (question == null)
                {
                    Console.WriteLine($"Question {answer.QuestionId} not found");
                    continue;
                }

                // Получаем все правильные опции для этого вопроса
                var correctOptionIds = question.Options
                    .Where(o => o.IsCorrect)
                    .Select(o => o.OptionId)
                    .ToHashSet();

                // Получаем выбранные пользователем опции (убираем дубликаты)
                var selectedOptionIds = answer.SelectedOptionIds.Distinct().ToHashSet();

                // Проверяем, правильно ли ответил пользователь
                // Ответ считается правильным, если выбраны ВСЕ правильные опции и НЕТ неправильных
                bool isQuestionAnsweredCorrectly = correctOptionIds.SetEquals(selectedOptionIds);
                
                if (isQuestionAnsweredCorrectly)
                {
                    correctAnswersCount++;
                    Console.WriteLine($"Question {answer.QuestionId} answered correctly");
                }
                else
                {
                    Console.WriteLine($"Question {answer.QuestionId} answered incorrectly. Correct: [{string.Join(", ", correctOptionIds)}], Selected: [{string.Join(", ", selectedOptionIds)}]");
                }

                // Сохраняем все выбранные опции пользователя
                foreach (var optionId in selectedOptionIds)
                {
                    if (optionMap.TryGetValue(optionId, out var option) &&
                        option.QuestionId == answer.QuestionId)
                    {
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
            
            // Вычисляем окончательный балл как процент правильных ответов
            int finalScore = totalQuestions > 0
                ? (int)Math.Round(100.0 * correctAnswersCount / totalQuestions)
                : 0;

            // Получаем проходной балл из теста
            int passingScore = test.PassScore;
            
            submission.Score = finalScore;
            submission.IsPassed = finalScore >= passingScore;
            
            Console.WriteLine($"Final Score: {finalScore}%, Correct Questions: {correctAnswersCount}/{totalQuestions}, Passed: {submission.IsPassed}, Passing Score: {passingScore}%");
            
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
            
            // Возвращаем результат с правильными данными
            return new TestSubmissionResultDto
            {
                Score = finalScore, // Окончательный балл (процент правильных ответов)
                scorePercentage = finalScore, // Для совместимости с фронтендом
                correctAnswers = correctAnswersCount, // Количество правильно отвеченных вопросов
                totalQuestions = totalQuestions, // Общее количество вопросов
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
}