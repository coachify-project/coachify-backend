using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coachify.BLL.Interfaces;
using Coachify.DAL;
using Coachify.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Coachify.BLL.Services
{
    public class ProgressService : IProgressService
    {
        private readonly ApplicationDbContext _db;

        public ProgressService(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IEnumerable<UserLessonProgress>> GetUserLessonProgressAsync(int userId, int moduleId)
        {
            var module = await _db.Modules
                .Include(m => m.Lessons)
                .FirstOrDefaultAsync(m => m.ModuleId == moduleId);

            if (module == null)
                throw new ArgumentException($"Модуль с ID {moduleId} не найден");

            var lessonIds = module.Lessons.Select(l => l.LessonId).ToList();

            var progresses = await _db.UserLessonProgresses
                .Where(p => p.UserId == userId && lessonIds.Contains(p.LessonId))
                .ToListAsync();

            return progresses;
        }

        public async Task<bool> StartLessonAsync(int userId, int lessonId)
        {
            // Проверяем пользователя и урок
            var user = await _db.Users.FindAsync(userId)
                ?? throw new ArgumentException($"User {userId} not found");
            var lesson = await _db.Lessons
                .Include(l => l.Module)
                .ThenInclude(m => m.Course)
                .FirstOrDefaultAsync(l => l.LessonId == lessonId)
                ?? throw new ArgumentException($"Lesson {lessonId} not found");

            // Проверяем enrollment
            var enr = await _db.Enrollments
                .FirstOrDefaultAsync(e => e.UserId == userId && e.CourseId == lesson.Module.CourseId)
                ?? throw new ArgumentException($"User {userId} is not enrolled in course");

            // Получаем или создаём прогресс
            var prog = await _db.UserLessonProgresses
                .FirstOrDefaultAsync(p => p.UserId == userId && p.LessonId == lessonId);
            if (prog == null)
            {
                prog = new UserLessonProgress
                {
                    UserId    = userId,
                    LessonId  = lessonId,
                    StatusId  = 3,              // InProgress
                    UpdatedAt = DateTime.UtcNow
                };
                _db.UserLessonProgresses.Add(prog);
            }
            else
            {
                prog.StatusId  = 3;          // InProgress
                prog.UpdatedAt = DateTime.UtcNow;
            }

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CompleteLessonAsync(int userId, int lessonId)
        {
            var prog = await _db.UserLessonProgresses
                .FirstOrDefaultAsync(p => p.UserId == userId && p.LessonId == lessonId)
                ?? throw new ArgumentException($"Lesson {lessonId} was not started by user {userId}");

            prog.StatusId  = 4;              // Completed
            prog.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> StartModuleAsync(int userId, int moduleId)
        {
            var user = await _db.Users.FindAsync(userId)
                ?? throw new ArgumentException($"User {userId} not found");
            var module = await _db.Modules
                .Include(m => m.Course)
                .FirstOrDefaultAsync(m => m.ModuleId == moduleId)
                ?? throw new ArgumentException($"Module {moduleId} not found");

            var enr = await _db.Enrollments
                .FirstOrDefaultAsync(e => e.UserId == userId && e.CourseId == module.CourseId)
                ?? throw new ArgumentException($"User {userId} is not enrolled in course");

            var prog = await _db.UserModuleProgresses
                .FirstOrDefaultAsync(p => p.UserId == userId && p.ModuleId == moduleId);
            if (prog == null)
            {
                prog = new UserModuleProgress
                {
                    UserId    = userId,
                    ModuleId  = moduleId,
                    StatusId  = 3,          // InProgress
                    UpdatedAt = DateTime.UtcNow
                };
                _db.UserModuleProgresses.Add(prog);
            }
            else
            {
                prog.StatusId  = 3;      // InProgress
                prog.UpdatedAt = DateTime.UtcNow;
            }

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CompleteModuleAsync(int userId, int moduleId)
        {
            var prog = await _db.UserModuleProgresses
                .FirstOrDefaultAsync(p => p.UserId == userId && p.ModuleId == moduleId)
                ?? throw new ArgumentException($"Module {moduleId} was not started by user {userId}");

            prog.StatusId  = 4;          // Completed
            prog.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<int>> GetCompletedLessonsAsync(int userId, int courseId)
        {
            return await _db.UserLessonProgresses
                .Where(p => p.UserId == userId && p.StatusId == 4)
                .Join(_db.Lessons,
                      prog   => prog.LessonId,
                      lesson => lesson.LessonId,
                      (prog, lesson) => new { prog, lesson })
                .Where(x => x.lesson.Module.CourseId == courseId)
                .Select(x => x.lesson.LessonId)
                .ToListAsync();
        }

        public async Task<IEnumerable<int>> GetCompletedModulesAsync(int userId, int courseId)
        {
            return await _db.UserModuleProgresses
                .Where(p => p.UserId == userId && p.StatusId == 4)
                .Join(_db.Modules,
                      prog    => prog.ModuleId,
                      module  => module.ModuleId,
                      (prog, module) => new { prog, module })
                .Where(x => x.module.CourseId == courseId)
                .Select(x => x.module.ModuleId)
                .ToListAsync();
        }
    }
}
