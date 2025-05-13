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

    public async Task<IEnumerable<ModuleDto>> GetAllAsync() =>
        _mapper.Map<IEnumerable<ModuleDto>>(await _db.Modules.ToListAsync());

    public async Task<ModuleDto?> GetByIdAsync(int id)
    {
        var e = await _db.Modules.FindAsync(id);
        return e == null ? null : _mapper.Map<ModuleDto>(e);
    }

    public async Task<ModuleDto> CreateAsync(CreateModuleDto dto)
    {
        var module = new Module
        {
            Title = dto.Title,
            CourseId = dto.CourseId,
            StatusId = dto.StatusId
        };

        // Добавление навыков (если есть)
        foreach (var skillName in dto.SkillNames.Distinct())
        {
            var existingSkill = await _db.Skills
                .FirstOrDefaultAsync(s => s.Name.ToLower() == skillName.ToLower());

            if (existingSkill != null)
            {
                module.Skills.Add(existingSkill);
            }
            else
            {
                var newSkill = new Skill { Name = skillName };
                _db.Skills.Add(newSkill);
                module.Skills.Add(newSkill);
            }
        }

        _db.Modules.Add(module);
        await _db.SaveChangesAsync();

        return new ModuleDto
        {
            ModuleId = module.ModuleId,
            Title = module.Title,
            CourseId = module.CourseId,
            StatusId = module.StatusId,
            SkillNames = module.Skills.Select(s => s.Name).ToList()
        };
    }


    public async Task UpdateAsync(int id, UpdateModuleDto dto)
    {
        var e = await _db.Modules.FindAsync(id);
        if (e == null) return;
        _mapper.Map(dto, e);
        await _db.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var e = await _db.Modules.FindAsync(id);
        if (e == null) return false;
        _db.Modules.Remove(e);
        await _db.SaveChangesAsync();
        return true;
    }
}