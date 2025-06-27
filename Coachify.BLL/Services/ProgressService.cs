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

            // Автоматически начинаем курс если он не начат
            await AutoStartCourseAsync(userId, lesson.Module.CourseId);

            // Автоматически начинаем модуль если он не начат
            await AutoStartModuleAsync(userId, lesson.ModuleId);

            // Получаем или создаём прогресс урока
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

            // Проверяем и автоматически завершаем модуль, если все уроки пройдены
            var lesson = await _db.Lessons
                .Include(l => l.Module)
                .FirstOrDefaultAsync(l => l.LessonId == lessonId);

            if (lesson != null)
            {
                await CheckAndCompleteModuleAsync(userId, lesson.ModuleId);
            }

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

            // Автоматически начинаем курс если он не начат
            await AutoStartCourseAsync(userId, module.CourseId);

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

            // Проверяем и автоматически завершаем курс, если все модули пройдены
            var module = await _db.Modules
                .FirstOrDefaultAsync(m => m.ModuleId == moduleId);

            if (module != null)
            {
                await CheckAndCompleteCourseAsync(userId, module.CourseId);
            }

            return true;
        }

        // НОВЫЙ МЕТОД ДЛЯ НАЧАЛА КУРСА
        public async Task<bool> StartCourseAsync(int userId, int courseId)
        {
            var user = await _db.Users.FindAsync(userId)
                ?? throw new ArgumentException($"User {userId} not found");

            var course = await _db.Courses.FindAsync(courseId)
                ?? throw new ArgumentException($"Course {courseId} not found");

            var enrollment = await _db.Enrollments
                .FirstOrDefaultAsync(e => e.UserId == userId && e.CourseId == courseId)
                ?? throw new ArgumentException($"User {userId} is not enrolled in course {courseId}");
            
            enrollment.StatusId = 2; // In Progress

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

        
        public async Task<int> GetLessonProgressStatusAsync(int userId, int lessonId)
        {
            var progress = await _db.UserLessonProgresses
                .FirstOrDefaultAsync(p => p.UserId == userId && p.LessonId == lessonId);

            return progress?.StatusId ?? 1; // "Not Started" 
        }
        
        public async Task<int> GetModuleProgressStatusAsync(int userId, int moduleId)
        {
            var progress = await _db.UserModuleProgresses
                .FirstOrDefaultAsync(p => p.UserId == userId && p.ModuleId == moduleId);

            if (progress != null)
            {
                return progress.StatusId;
            }

            var module = await _db.Modules
                .Include(m => m.Lessons)
                .FirstOrDefaultAsync(m => m.ModuleId == moduleId);

            if (module == null || !module.Lessons.Any())
                return 1; // Not Started

            var lessonIds = module.Lessons.Select(l => l.LessonId).ToList();
            var lessonProgresses = await _db.UserLessonProgresses
                .Where(p => p.UserId == userId && lessonIds.Contains(p.LessonId))
                .ToListAsync();

            if (!lessonProgresses.Any())
                return 1; // Not Started

            if (lessonProgresses.Count == lessonIds.Count && 
                lessonProgresses.All(p => p.StatusId == 4))
                return 3; // Completed

            return 2; // In Progress
        }
        
        public async Task<int> GetCourseProgressStatusAsync(int userId, int courseId)
        {
            var enrollment = await _db.Enrollments
                .FirstOrDefaultAsync(e => e.UserId == userId && e.CourseId == courseId);

            if (enrollment == null)
                return 1; // Not Started

            return enrollment.StatusId;
        }
        
        public async Task<int> GetCourseProgressPercentageAsync(int userId, int courseId)
        {
            var completedLessons = await GetCompletedLessonsAsync(userId, courseId);
            var totalLessons = await _db.Lessons
                .Where(l => l.Module.CourseId == courseId)
                .CountAsync();

            if (totalLessons == 0)
                return 0;

            return (int)Math.Round((double)completedLessons.Count() / totalLessons * 100);
        }

        
        public async Task<Dictionary<int, int>> GetModuleLessonsProgressStatusAsync(int userId, int moduleId)
        {
            var module = await _db.Modules
                .Include(m => m.Lessons)
                .FirstOrDefaultAsync(m => m.ModuleId == moduleId);

            if (module == null)
                return new Dictionary<int, int>();

            var lessonIds = module.Lessons.Select(l => l.LessonId).ToList();
            var progresses = await _db.UserLessonProgresses
                .Where(p => p.UserId == userId && lessonIds.Contains(p.LessonId))
                .ToListAsync();

            var result = new Dictionary<int, int>();
            foreach (var lessonId in lessonIds)
            {
                var progress = progresses.FirstOrDefault(p => p.LessonId == lessonId);
                result[lessonId] = progress?.StatusId ?? 1; // Not Started
            }

            return result;
        }
        
        public async Task<Dictionary<int, int>> GetCourseModulesProgressStatusAsync(int userId, int courseId)
        {
            var modules = await _db.Modules
                .Where(m => m.CourseId == courseId)
                .ToListAsync();

            var result = new Dictionary<int, int>();
            foreach (var module in modules)
            {
                result[module.ModuleId] = await GetModuleProgressStatusAsync(userId, module.ModuleId);
            }

            return result;
        }
        
        private async Task AutoStartCourseAsync(int userId, int courseId)
        {
            var enrollment = await _db.Enrollments
                .FirstOrDefaultAsync(e => e.UserId == userId && e.CourseId == courseId);

            if (enrollment != null && enrollment.StatusId == 1) // Not Started
            {
                enrollment.StatusId = 2; // In Progress
            }
        }

       
        private async Task AutoStartModuleAsync(int userId, int moduleId)
        {
            var progress = await _db.UserModuleProgresses
                .FirstOrDefaultAsync(p => p.UserId == userId && p.ModuleId == moduleId);

            if (progress == null)
            {
                progress = new UserModuleProgress
                {
                    UserId = userId,
                    ModuleId = moduleId,
                    StatusId = 2, // In Progress
                    UpdatedAt = DateTime.UtcNow
                };
                _db.UserModuleProgresses.Add(progress);
            }
            else if (progress.StatusId == 1) // Not Started
            {
                progress.StatusId = 2; // In Progress
                progress.UpdatedAt = DateTime.UtcNow;
            }
        }

        
        private async Task CheckAndCompleteModuleAsync(int userId, int moduleId)
        {
            var module = await _db.Modules
                .Include(m => m.Lessons)
                .FirstOrDefaultAsync(m => m.ModuleId == moduleId);

            if (module == null || !module.Lessons.Any())
                return;

            var lessonIds = module.Lessons.Select(l => l.LessonId).ToList();
            var completedLessons = await _db.UserLessonProgresses
                .Where(p => p.UserId == userId && lessonIds.Contains(p.LessonId) && p.StatusId == 4)
                .CountAsync();

            if (completedLessons == lessonIds.Count)
            {
                var moduleProgress = await _db.UserModuleProgresses
                    .FirstOrDefaultAsync(p => p.UserId == userId && p.ModuleId == moduleId);

                if (moduleProgress != null && moduleProgress.StatusId != 3) // Not Completed yet
                {
                    moduleProgress.StatusId = 3; // Completed
                    moduleProgress.UpdatedAt = DateTime.UtcNow;

                    await CheckAndCompleteCourseAsync(userId, module.CourseId);
                }
            }
        }
        
        private async Task CheckAndCompleteCourseAsync(int userId, int courseId)
        {
            var modules = await _db.Modules
                .Where(m => m.CourseId == courseId)
                .ToListAsync();

            if (!modules.Any())
                return;

            var moduleIds = modules.Select(m => m.ModuleId).ToList();
            var completedModules = await _db.UserModuleProgresses
                .Where(p => p.UserId == userId && moduleIds.Contains(p.ModuleId) && p.StatusId == 3)
                .CountAsync();

            if (completedModules == moduleIds.Count)
            {
                var enrollment = await _db.Enrollments
                    .FirstOrDefaultAsync(e => e.UserId == userId && e.CourseId == courseId);

                if (enrollment != null && enrollment.StatusId != 3) // Not Completed yet
                {
                    enrollment.StatusId = 3; // Completed
                }
            }
        }
    }
}