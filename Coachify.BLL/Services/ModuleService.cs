using AutoMapper;
using Coachify.BLL.DTOs.Module;
using Coachify.BLL.DTOs.Progress;
using Coachify.BLL.DTOs.Question;
using Coachify.BLL.DTOs.Test;
using Coachify.BLL.Interfaces;
using Coachify.DAL;
using Coachify.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Coachify.BLL.Services
{
    public class ModuleService : IModuleService
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public ModuleService(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ModuleDto>> GetAllAsync()
        {
            var modules = await _db.Modules.Include(m => m.Skills).ToListAsync();
            return _mapper.Map<IEnumerable<ModuleDto>>(modules);
        }

        public async Task<IEnumerable<ModuleDto>> GetAllByCourseAsync(int courseId)
        {
            var modules = await _db.Modules
                .Where(m => m.CourseId == courseId)
                .Include(m => m.Skills)
                .ToListAsync();
            return _mapper.Map<IEnumerable<ModuleDto>>(modules);
        }

        public async Task<ModuleDto?> GetByIdAsync(int id)
        {
            var module = await _db.Modules
                .Include(m => m.Skills)
                .Include(m => m.Lessons)
                .Include(m => m.Test)
                .FirstOrDefaultAsync(m => m.ModuleId == id);
            return module == null ? null : _mapper.Map<ModuleDto>(module);
        }

        public async Task<ModuleDto?> GetByIdForUserAsync(int moduleId, int userId)
        {
            var module = await _db.Modules
                .Include(m => m.Skills)
                .Include(m => m.Lessons)
                .Include(m => m.Test)
                .FirstOrDefaultAsync(m => m.ModuleId == moduleId);

            if (module == null) return null;

            var progress = await _db.UserModuleProgresses
                .Include(p => p.Status)
                .FirstOrDefaultAsync(p => p.ModuleId == moduleId && p.UserId == userId);

            ProgressStatusDto statusDto;
            if (progress == null)
            {
                var defaultStatus = await _db.ProgressStatuses.FirstAsync(s => s.StatusId == 2);
                statusDto = _mapper.Map<ProgressStatusDto>(defaultStatus);
            }
            else
            {
                statusDto = _mapper.Map<ProgressStatusDto>(progress.Status);
            }

            var moduleDto = _mapper.Map<ModuleDto>(module);
            moduleDto.Status = statusDto;
            return moduleDto;
        }

        public async Task<TestDto?> GetTestByModuleForUserAsync(int userId, int moduleId)
        {
            var user = await _db.Users.FindAsync(userId);
            if (user == null)
                throw new ArgumentException($"Пользователь с ID {userId} не найден");

            var module = await _db.Modules
                .Include(m => m.Course)
                .Include(m => m.Test)
                .FirstOrDefaultAsync(m => m.ModuleId == moduleId);
            if (module == null)
                throw new ArgumentException($"Модуль с ID {moduleId} не найден");

            var enrollment = await _db.Enrollments
                .FirstOrDefaultAsync(e => e.UserId == userId && e.CourseId == module.CourseId);
            if (enrollment == null)
                throw new InvalidOperationException("Пользователь не зачислен на курс, содержащий этот модуль");

            if (module.Test == null)
                return null;

            var lessonIds = module.Lessons.Select(l => l.LessonId).ToList();

            var completedLessonsCount = await _db.UserLessonProgresses
                .CountAsync(lp => lp.UserId == userId
                                  && lessonIds.Contains(lp.LessonId)
                                  && lp.StatusId == 4);

            if (completedLessonsCount < lessonIds.Count)
            {
                return new TestDto
                {
                    Id = module.Test.TestId,
                    Title = module.Test.Title,
                    ModuleId = module.ModuleId,
                    Questions = new List<QuestionDto>()
                };
            }

            return _mapper.Map<TestDto>(module.Test);
        }


        public async Task<ModuleDto> CreateAsync(CreateModuleDto dto)
        {
            var course = await _db.Courses.FindAsync(dto.CourseId);
            if (course == null)
                throw new KeyNotFoundException("Course not found.");
            if (course.StatusId != 1 && course.StatusId != 4)
                throw new InvalidOperationException(
                    "Добавлять модули можно только в курсе в черновике или после отклонения.");

            var existingSkills = await _db.Skills.Where(s => dto.SkillNames.Contains(s.Name)).ToListAsync();

            var missingNames = dto.SkillNames
                .Except(existingSkills.Select(s => s.Name), StringComparer.OrdinalIgnoreCase)
                .Distinct(StringComparer.OrdinalIgnoreCase);

            var newSkills = missingNames.Select(name => new Skill { Name = name }).ToList();

            if (newSkills.Any())
            {
                _db.Skills.AddRange(newSkills);
                await _db.SaveChangesAsync();
                existingSkills.AddRange(newSkills);
            }

            var module = new Module
            {
                CourseId = dto.CourseId,
                Title = dto.Title,
                Description = dto.Description,
                Skills = existingSkills,
                TestId = dto.TestId
            };

            _db.Modules.Add(module);
            await _db.SaveChangesAsync();

            return _mapper.Map<ModuleDto>(module);
        }

        public async Task<ModuleDto> UpdateAsync(int id, UpdateModuleDto dto)
        {
            var module = await _db.Modules.FindAsync(id);
            if (module == null)
                throw new KeyNotFoundException("Module not found.");

            var course = await _db.Courses.FindAsync(module.CourseId);
            if (course.StatusId != 1 && course.StatusId != 4)
                throw new InvalidOperationException(
                    "Изменять модули можно только в курсе в черновике или после отклонения.");

            var existingSkills = await _db.Skills.Where(s => dto.SkillNames.Contains(s.Name)).ToListAsync();

            var missingNames = dto.SkillNames
                .Except(existingSkills.Select(s => s.Name), StringComparer.OrdinalIgnoreCase)
                .Distinct(StringComparer.OrdinalIgnoreCase);

            var newSkills = missingNames.Select(name => new Skill { Name = name }).ToList();

            if (newSkills.Any())
            {
                _db.Skills.AddRange(newSkills);
                await _db.SaveChangesAsync();
                existingSkills.AddRange(newSkills);
            }

            module.Title = dto.Title;
            module.Description = dto.Description;
            module.Skills = existingSkills;
            module.TestId = dto.TestId;

            await _db.SaveChangesAsync();
            return _mapper.Map<ModuleDto>(module);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var module = await _db.Modules.FindAsync(id);
            if (module == null)
                return false;

            var course = await _db.Courses.FindAsync(module.CourseId);
            if (course.StatusId != 1 && course.StatusId != 4)
                throw new InvalidOperationException(
                    "Удалять модули можно только в курсе в черновике или после отклонения.");

            _db.Modules.Remove(module);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> StartModuleAsync(int userId, int moduleId)
        {
            var user = await _db.Users.FindAsync(userId);
            if (user == null)
                throw new ArgumentException($"Пользователь с ID {userId} не найден");

            var module = await _db.Modules.Include(m => m.Course).FirstOrDefaultAsync(m => m.ModuleId == moduleId);
            if (module == null)
                throw new ArgumentException($"Модуль с ID {moduleId} не найден");

            var enrollment =
                await _db.Enrollments.FirstOrDefaultAsync(e => e.UserId == userId && e.CourseId == module.CourseId);
            if (enrollment == null)
                throw new ArgumentException($"Пользователь не зачислен на курс, содержащий этот модуль");

            var progress =
                await _db.UserModuleProgresses.FirstOrDefaultAsync(p => p.UserId == userId && p.ModuleId == moduleId);
            if (progress == null)
            {
                progress = new UserModuleProgress
                {
                    UserId = userId,
                    ModuleId = moduleId,
                    StatusId = 3,
                    UpdatedAt = DateTime.UtcNow
                };
                _db.UserModuleProgresses.Add(progress);
            }
            else
            {
                progress.StatusId = 3;
                progress.UpdatedAt = DateTime.UtcNow;
            }

            if (enrollment.StatusId == 1)
            {
                enrollment.StatusId = 2;
            }

            await _db.SaveChangesAsync();
            return true;
        }


        public async Task<bool> MarkLessonCompletedAsync(int userId, int lessonId)
        {
            var user = await _db.Users.FindAsync(userId);
            if (user == null)
                throw new ArgumentException($"Пользователь с ID {userId} не найден");

            var lesson = await _db.Lessons.Include(l => l.Module).ThenInclude(m => m.Course)
                .FirstOrDefaultAsync(l => l.LessonId == lessonId);
            if (lesson == null)
                throw new ArgumentException($"Урок с ID {lessonId} не найден");

            var enrollment = await _db.Enrollments
                .FirstOrDefaultAsync(e => e.UserId == userId && e.CourseId == lesson.Module.CourseId);
            if (enrollment == null)
                throw new InvalidOperationException("Пользователь не зачислен на курс, содержащий этот урок");

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
                progress.StatusId = 4; // Completed
                progress.UpdatedAt = DateTime.UtcNow;
            }

            await _db.SaveChangesAsync();
            return true;
        }


        public async Task<IEnumerable<UserLessonProgress>> GetUserLessonProgressAsync(int userId, int moduleId)
        {
            var module = await _db.Modules.Include(m => m.Lessons)
                .FirstOrDefaultAsync(m => m.ModuleId == moduleId);

            if (module == null)
                throw new ArgumentException($"Модуль с ID {moduleId} не найден");

            var lessonIds = module.Lessons.Select(l => l.LessonId).ToList();

            var progresses = await _db.UserLessonProgresses
                .Where(p => p.UserId == userId && lessonIds.Contains(p.LessonId))
                .ToListAsync();

            return progresses;
        }


        public async Task<bool> CompleteModuleAsync(int userId, int moduleId)
        {
            var user = await _db.Users.FindAsync(userId);
            if (user == null)
                throw new ArgumentException($"Пользователь с ID {userId} не найден");

            var module = await _db.Modules
                .Include(m => m.Lessons)
                .Include(m => m.Test)
                .FirstOrDefaultAsync(m => m.ModuleId == moduleId);
            if (module == null)
                throw new ArgumentException($"Модуль с ID {moduleId} не найден");

            var lessonIds = module.Lessons.Select(l => l.LessonId).ToList();

            var completedLessonsCount = await _db.UserLessonProgresses
                .CountAsync(lp => lp.UserId == userId && lessonIds.Contains(lp.LessonId) && lp.StatusId == 4);

            if (completedLessonsCount < lessonIds.Count)
                throw new InvalidOperationException("Все уроки модуля должны быть пройдены перед завершением модуля.");

            if (module.Test != null)
            {
                bool testPassed = await _db.TestSubmissions
                    .AnyAsync(ts => ts.TestId == module.Test.TestId && ts.UserId == userId && ts.IsPassed);

                if (!testPassed)
                    throw new InvalidOperationException(
                        "Тест модуля должен быть успешно пройден перед завершением модуля.");
            }

            // Обновляем прогресс пользователя по модулю
            var progress = await _db.UserModuleProgresses
                .FirstOrDefaultAsync(p => p.UserId == userId && p.ModuleId == moduleId);

            if (progress == null)
            {
                progress = new UserModuleProgress
                {
                    UserId = userId,
                    ModuleId = moduleId,
                    StatusId = 4, // Completed
                    UpdatedAt = DateTime.UtcNow
                };
                _db.UserModuleProgresses.Add(progress);
            }
            else
            {
                progress.StatusId = 4;
                progress.UpdatedAt = DateTime.UtcNow;
            }

            await _db.SaveChangesAsync();
            return true;
        }
    }
}