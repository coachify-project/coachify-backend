// TestSubmissionService.cs
using AutoMapper;
using Coachify.BLL.DTOs.TestSubmission;
using Coachify.DAL;
using Coachify.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coachify.BLL.Interfaces;

public class TestSubmissionService : ITestSubmissionService
{
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;
    private readonly IEnrollmentService _enrollmentService;

    public TestSubmissionService(ApplicationDbContext db, IMapper mapper, IEnrollmentService enrollmentService)
    {
        _db = db;
        _mapper = mapper;
        _enrollmentService = enrollmentService;
    }

    public async Task<IEnumerable<TestSubmissionDto>> GetAllAsync()
    {
        var submissions = await _db.TestSubmissions
            .AsNoTracking()
            .ToListAsync();

        return _mapper.Map<IEnumerable<TestSubmissionDto>>(submissions);
    }

    public async Task<TestSubmissionDto?> GetByIdAsync(int id)
    {
        var s = await _db.TestSubmissions.FindAsync(id);
        if (s == null) return null;

        // Сколько вопросов в тесте
        int total = await _db.Questions.CountAsync(q => q.TestId == s.TestId);
        // Сколько правильных ответов выбрал пользователь
        int correct = await _db.TestSubmissionAnswers
            .Where(a => a.SubmissionId == id && a.Option.IsCorrect)
            .CountAsync();

        var dto = _mapper.Map<TestSubmissionDto>(s);
        dto.TotalQuestions = total;
        dto.CorrectAnswers = correct;
        return dto;
    }

   public async Task<TestSubmissionDto> CreateAsync(CreateTestSubmissionDto dto)
    {
        // 1) Подгружаем вопросы и опции теста
        var questions = await _db.Questions
            .Where(q => q.TestId == dto.TestId)
            .Include(q => q.Options)
            .ToListAsync();

        if (!questions.Any())
            throw new KeyNotFoundException("Test or questions not found");

        // 2) Создаём TestSubmission
        var submission = new TestSubmission
        {
            TestId      = dto.TestId,
            UserId      = dto.UserId,
            SubmittedAt = dto.SubmittedAt
        };
        _db.TestSubmissions.Add(submission);
        await _db.SaveChangesAsync();

        // 3) Сохраняем ответы и считаем правильные
        int totalQuestions = questions.Count;
        int correctAnswers = 0;

        var optionMap = questions
            .SelectMany(q => q.Options)
            .ToDictionary(o => o.OptionId);

        foreach (var optId in dto.SelectedOptionIds.Distinct())
        {
            if (optionMap.TryGetValue(optId, out var opt))
            {
                if (opt.IsCorrect) correctAnswers++;

                _db.TestSubmissionAnswers.Add(new TestSubmissionAnswer
                {
                    SubmissionId = submission.SubmissionId,
                    QuestionId   = opt.QuestionId,
                    OptionId     = optId
                });
            }
        }

        // 4) Считаем Score и IsPassed
        submission.Score    = totalQuestions > 0
            ? (int)Math.Round(100.0 * correctAnswers / totalQuestions)
            : 0;
        submission.IsPassed = submission.Score >= 70;

        await _db.SaveChangesAsync();

        // 5) Проверяем и завершаем курс, если все тесты пройдены
        var enrollment = await _db.Enrollments
            .Include(e => e.Course)
                .ThenInclude(c => c.Modules)
                    .ThenInclude(m => m.Test)
            .FirstOrDefaultAsync(e =>
                e.CourseId == submission.Test.Module.CourseId &&
                e.UserId == dto.UserId);

        if (enrollment != null && submission.IsPassed)
        {
            bool allPassed = true;
            foreach (var module in enrollment.Course.Modules)
            {
                if (module.Test == null) continue;
                bool passed = await _db.TestSubmissions
                    .AnyAsync(ts =>
                        ts.TestId == module.Test.TestId &&
                        ts.UserId == dto.UserId &&
                        ts.IsPassed);
                if (!passed) { allPassed = false; break; }
            }
            if (allPassed && enrollment.StatusId != 3) // 3 = Completed
                await _enrollmentService.CompleteEnrollmentAsync(enrollment.EnrollmentId);
        }

        // 6) Мапим в DTO и возвращаем
        var dtoResult = _mapper.Map<TestSubmissionDto>(submission);
        dtoResult.TotalQuestions = totalQuestions;
        dtoResult.CorrectAnswers = correctAnswers;
        return dtoResult;
    }
   

    public async Task UpdateAsync(int id, UpdateTestSubmissionDto dto)
    {
        var submission = await _db.TestSubmissions.FindAsync(id);
        if (submission == null)
            throw new KeyNotFoundException($"TestSubmission with id={id} not found");

        _mapper.Map(dto, submission);
        await _db.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var submission = await _db.TestSubmissions.FindAsync(id);
        if (submission == null) return false;

        _db.TestSubmissions.Remove(submission);
        await _db.SaveChangesAsync();
        return true;
    }
}
