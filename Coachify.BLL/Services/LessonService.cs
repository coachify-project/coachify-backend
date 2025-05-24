using AutoMapper;
using Coachify.BLL.DTOs.Lesson;
using Coachify.BLL.Interfaces;
using Coachify.DAL;
using Coachify.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coachify.BLL.Services
{
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

            if (module == null)
                throw new KeyNotFoundException("Module not found.");

            if (module.Course.StatusId != 1 && module.Course.StatusId != 4)
                throw new InvalidOperationException(
                    "Добавлять уроки можно только в курсе в черновике или после отклонения.");

            string videoUrl = ProcessVideoUrl(dto.VideoUrl);

            var lesson = new Lesson
            {
                Title = dto.Title,
                Introduction = dto.Introduction,
                LessonObjectives = dto.LessonObjectives,
                VideoUrl = videoUrl,
                ModuleId = dto.ModuleId
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
            var user = await _db.Users.FindAsync(userId);
            if (user == null)
                throw new ArgumentException($"Пользователь с ID {userId} не найден");

            var lesson = await _db.Lessons
                .Include(l => l.Module)
                .ThenInclude(m => m.Course)
                .FirstOrDefaultAsync(l => l.LessonId == lessonId);

            if (lesson == null)
                throw new ArgumentException($"Урок с ID {lessonId} не найден");

            var enrollment = await _db.Enrollments
                .FirstOrDefaultAsync(e => e.UserId == userId && e.CourseId == lesson.Module.CourseId);

            if (enrollment == null)
                throw new InvalidOperationException("Пользователь не зачислен на курс.");

            var progress = await _db.UserLessonProgresses
                .FirstOrDefaultAsync(p => p.UserId == userId && p.LessonId == lessonId);

            if (progress == null)
            {
                progress = new UserLessonProgress
                {
                    UserId = userId,
                    LessonId = lessonId,
                    StatusId = 3, // In Progress
                    UpdatedAt = DateTime.UtcNow
                };
                _db.UserLessonProgresses.Add(progress);
            }
            else if (progress.StatusId == 2) // Not Started
            {
                progress.StatusId = 3;
                progress.UpdatedAt = DateTime.UtcNow;
            }

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CompleteLessonAsync(int userId, int lessonId)
        {
            var lesson = await _db.Lessons
                .Include(l => l.Module)
                .ThenInclude(m => m.Course)
                .FirstOrDefaultAsync(l => l.LessonId == lessonId);

            if (lesson == null)
                throw new ArgumentException($"Урок с ID {lessonId} не найден");

            var enrollment = await _db.Enrollments
                .FirstOrDefaultAsync(e => e.UserId == userId && e.CourseId == lesson.Module.CourseId);

            if (enrollment == null)
                throw new InvalidOperationException("Пользователь не зачислен на курс.");

            var progress = await _db.UserLessonProgresses
                .FirstOrDefaultAsync(p => p.UserId == userId && p.LessonId == lessonId);

            if (progress == null)
            {
                progress = new UserLessonProgress
                {
                    UserId = userId,
                    LessonId = lessonId,
                    StatusId = 4, // Completed
                    UpdatedAt = DateTime.UtcNow
                };
                _db.UserLessonProgresses.Add(progress);
            }
            else
            {
                progress.StatusId = 4;
                progress.UpdatedAt = DateTime.UtcNow;
            }

            // Check if all lessons in module are completed
            var lessonIdsInModule = await _db.Lessons
                .Where(l => l.ModuleId == lesson.ModuleId)
                .Select(l => l.LessonId)
                .ToListAsync();

            var completedCount = await _db.UserLessonProgresses
                .CountAsync(p => p.UserId == userId &&
                                 lessonIdsInModule.Contains(p.LessonId) &&
                                 p.StatusId == 4);

            if (completedCount == lessonIdsInModule.Count)
            {
                var moduleProgress = await _db.UserModuleProgresses
                    .FirstOrDefaultAsync(mp => mp.UserId == userId && mp.ModuleId == lesson.ModuleId);

                if (moduleProgress == null)
                {
                    _db.UserModuleProgresses.Add(new UserModuleProgress
                    {
                        UserId = userId,
                        ModuleId = lesson.ModuleId,
                        StatusId = 4,
                        UpdatedAt = DateTime.UtcNow
                    });
                }
                else
                {
                    moduleProgress.StatusId = 4;
                    moduleProgress.UpdatedAt = DateTime.UtcNow;
                }
            }

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<LessonDto> UpdateAsync(int id, UpdateLessonDto dto)
        {
            var lesson = await _db.Lessons
                .Include(l => l.Module)
                .ThenInclude(m => m.Course)
                .FirstOrDefaultAsync(l => l.LessonId == id);

            if (lesson == null)
                throw new KeyNotFoundException("Lesson not found.");

            var courseStatus = lesson.Module.Course.StatusId;

            if (courseStatus != 1 && courseStatus != 4)
                throw new InvalidOperationException(
                    "Изменять уроки можно только в курсе в черновике или после отклонения.");

            lesson.Title = dto.Title;
            lesson.Introduction = dto.Introduction;
            lesson.LessonObjectives = dto.LessonObjectives;
            lesson.VideoUrl = dto.VideoUrl;

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

        public async Task InitializeLessonProgressForEnrollment(int userId, int courseId)
        {
            var lessonIds = await _db.Lessons
                .Where(l => l.Module.CourseId == courseId)
                .Select(l => l.LessonId)
                .ToListAsync();

            foreach (var lessonId in lessonIds)
            {
                bool exists = await _db.UserLessonProgresses
                    .AnyAsync(p => p.UserId == userId && p.LessonId == lessonId);

                if (!exists)
                {
                    _db.UserLessonProgresses.Add(new UserLessonProgress
                    {
                        UserId = userId,
                        LessonId = lessonId,
                        StatusId = 2, // Not Started
                        UpdatedAt = DateTime.UtcNow
                    });
                }
            }

            await _db.SaveChangesAsync();
        }
    }
}
