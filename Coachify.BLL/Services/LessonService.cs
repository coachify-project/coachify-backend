using AutoMapper;
using Coachify.BLL.DTOs.Lesson;
using Coachify.BLL.Interfaces;
using Coachify.DAL;
using Coachify.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Coachify.BLL.Services;

public class LessonService : ILessonService
{
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;

    public LessonService(ApplicationDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<IEnumerable<LessonDto>> GetAllAsync() =>
        _mapper.Map<IEnumerable<LessonDto>>(await _db.Lessons.ToListAsync());

    public async Task<LessonDto?> GetByIdAsync(int id)
    {
        var lesson = await _db.Lessons.FindAsync(id);
        return lesson == null ? null : _mapper.Map<LessonDto>(lesson);
    }

    public async Task<LessonDto> CreateAsync(CreateLessonDto dto)
    {
        var module = await _db.Modules
            .Include(m => m.Course)
            .FirstOrDefaultAsync(m => m.ModuleId == dto.ModuleId);
        if (module == null) throw new KeyNotFoundException("Module not found.");
        if (module.Course.StatusId != 1 &&
            module.Course.StatusId != 4)
            throw new InvalidOperationException(
                "Добавлять уроки можно только в курсе в черновике или после отклонения.");

        // Пример: преобразование YouTube-ссылки в embed
        string videoUrl = ProcessVideoUrl(dto.VideoUrl);

        var lesson = new Lesson
        {
            Title = dto.Title,
            Introduction = dto.Introduction,
            LessonObjectives = dto.LessonObjectives,
            VideoUrl = videoUrl,
            ModuleId = dto.ModuleId,
            StatusId = 1
        };

        _db.Lessons.Add(lesson);
        await _db.SaveChangesAsync();

        return _mapper.Map<LessonDto>(lesson);
    }

    private string ProcessVideoUrl(string url)
    {
        if (url.Contains("youtube.com/watch?v="))
        {
            var videoId = url.Split("v=")[1].Split('&')[0];
            return $"https://www.youtube.com/embed/{videoId}";
        }

        return url;
    }

    public async Task<bool> StartLessonAsync(int userId, int lessonId)
    {
        // Проверка существования пользователя
        var user = await _db.Users.FindAsync(userId);
        if (user == null)
            throw new ArgumentException($"Пользователь с ID {userId} не найден");

        // Получаем урок и связанные с ним модуль и курс
        var lesson = await _db.Lessons
            .Include(l => l.Module)
            .ThenInclude(m => m.Course)
            .FirstOrDefaultAsync(l => l.LessonId == lessonId);

        if (lesson == null)
            throw new ArgumentException($"Урок с ID {lessonId} не найден");

        // Проверяем, что пользователь зачислен на курс
        var enrollment = await _db.Enrollments
            .FirstOrDefaultAsync(e => e.UserId == userId && e.CourseId == lesson.Module.CourseId);

        if (enrollment == null)
            throw new ArgumentException($"Пользователь не зачислен на курс, содержащий этот урок");

        // Проверяем, что урок не в статусе Draft
        if (lesson.StatusId == 1) // Draft
            throw new ArgumentException($"Урок находится в статусе Draft и не доступен для прохождения");

        // Обновляем статус урока на "In Progress"
        lesson.StatusId = 3; // Статус "In Progress"

        // Обновляем статус модуля на "In Progress", если он еще не начат
        if (lesson.Module.StatusId == 2) // Not Started
        {
            lesson.Module.StatusId = 3; // In Progress
        }

        // Если это первый урок, который пользователь начинает, 
        // обновляем статус зачисления на "In Progress"
        if (enrollment.StatusId == 1) // Not Started
        {
            enrollment.StatusId = 2; // In Progress
        }

        await _db.SaveChangesAsync();
        return true;
    }

    public Task<bool> CompleteLessonAsync(int userId, int lessonId)
    {
        throw new NotImplementedException();
    }


    public async Task<LessonDto> UpdateAsync(int id, UpdateLessonDto dto)
    {
        var lesson = await _db.Lessons
            .Include(l => l.Module)
            .ThenInclude(m => m.Course)
            .FirstOrDefaultAsync(l => l.LessonId == id);
        if (lesson == null) throw new KeyNotFoundException("Lesson not found.");
        var courseStatus = lesson.Module.Course.StatusId;
        if (courseStatus != 1 && courseStatus != 4)
            throw new InvalidOperationException(
                "Изменять уроки можно только в курсе в черновике или после отклонения.");

        // 2) Обновляем поля
        lesson.Title = dto.Title;
        lesson.Introduction = dto.Introduction;
        lesson.LessonObjectives = dto.LessonObjectives;
        lesson.VideoUrl = dto.VideoUrl;
        // статус урока не трогаем здесь
        await _db.SaveChangesAsync();

        return _mapper.Map<LessonDto>(lesson);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var lesson = await _db.Lessons
            .Include(l => l.Module)
            .ThenInclude(m => m.Course)
            .FirstOrDefaultAsync(l => l.LessonId == id);
        if (lesson == null) return false;
        var courseStatus = lesson.Module.Course.StatusId;
        if (courseStatus != 1 && courseStatus != 4)
            throw new InvalidOperationException(
                "Удалять уроки можно только в курсе в черновике или после отклонения.");

        _db.Lessons.Remove(lesson);
        await _db.SaveChangesAsync();
        return true;
    }
}