using AutoMapper;
using Coachify.BLL.DTOs.Module;
using Coachify.BLL.Interfaces;
using Coachify.DAL;
using Coachify.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Coachify.BLL.Services;

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
        var modules = await _db.Modules
            .Include(m => m.Skills)
            .ToListAsync();

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
            .FirstOrDefaultAsync(m => m.ModuleId == id);
        return module == null ? null : _mapper.Map<ModuleDto>(module);
    }

    public async Task<ModuleDto> CreateAsync(CreateModuleDto dto)
    {
        var course = await _db.Courses.FindAsync(dto.CourseId);
        if (course == null)
            throw new KeyNotFoundException("Course not found.");
        if (course.StatusId != 1 && course.StatusId != 4)
            throw new InvalidOperationException("Добавлять модули можно только в курсе в черновике или после отклонения.");

        var existingSkills = await _db.Skills
            .Where(s => dto.SkillNames.Contains(s.Name))
            .ToListAsync();

        var missingNames = dto.SkillNames
            .Except(existingSkills.Select(s => s.Name), StringComparer.OrdinalIgnoreCase)
            .Distinct(StringComparer.OrdinalIgnoreCase);

        // 3) Создаём новые Skill для отсутствующих имён
        var newSkills = missingNames
            .Select(name => new Skill { Name = name })
            .ToList();

        if (newSkills.Any())
        {
            _db.Skills.AddRange(newSkills);
            await _db.SaveChangesAsync();
            existingSkills.AddRange(newSkills);
        }

        var module = new Module
        {
            CourseId = dto.CourseId,
            StatusId = 1,
            Title = dto.Title,
            Description = dto.Description,
            Skills = existingSkills
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
            throw new InvalidOperationException("Изменять модули можно только в курсе в черновике или после отклонения.");

        // Повторяем ту же логику для навыков
        var existingSkills = await _db.Skills
            .Where(s => dto.SkillNames.Contains(s.Name))
            .ToListAsync();

        var missingNames = dto.SkillNames
            .Except(existingSkills.Select(s => s.Name), StringComparer.OrdinalIgnoreCase)
            .Distinct(StringComparer.OrdinalIgnoreCase);

        var newSkills = missingNames
            .Select(name => new Skill { Name = name })
            .ToList();

        if (newSkills.Any())
        {
            _db.Skills.AddRange(newSkills);
            await _db.SaveChangesAsync();
            existingSkills.AddRange(newSkills);
        }

        module.Title = dto.Title;
        module.Description = dto.Description;
        module.Skills = existingSkills;

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
            throw new InvalidOperationException("Удалять модули можно только в курсе в черновике или после отклонения.");

        // 2) удаляем
        _db.Modules.Remove(module);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> StartModuleAsync(int userId, int moduleId)
    {
        // Проверка существования пользователя
        var user = await _db.Users.FindAsync(userId);
        if (user == null)
            throw new ArgumentException($"Пользователь с ID {userId} не найден");

        // Получаем модуль и связанный с ним курс
        var module = await _db.Modules
            .Include(m => m.Course)
            .FirstOrDefaultAsync(m => m.ModuleId == moduleId);

        if (module == null)
            throw new ArgumentException($"Модуль с ID {moduleId} не найден");

        // Проверяем, что пользователь зачислен на курс
        var enrollment = await _db.Enrollments
            .FirstOrDefaultAsync(e => e.UserId == userId && e.CourseId == module.CourseId);

        if (enrollment == null)
            throw new ArgumentException($"Пользователь не зачислен на курс, содержащий этот модуль");

        // Обновляем статус модуля на "In Progress"
        module.StatusId = 3; // Статус "In progress"

        // Если это первый модуль, который пользователь начинает, 
        // обновляем статус зачисления на "In Progress"
        if (enrollment.StatusId == 1) // Not Started
        {
            enrollment.StatusId = 2; // In Progress
        }

        await _db.SaveChangesAsync();
        return true;
    }
}