using AutoMapper;
using Coachify.BLL.DTOs.LessonStatus;
using Coachify.BLL.Interfaces;
using Coachify.DAL;
using Coachify.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Coachify.BLL.Services;

public class LessonStatusService : ILessonStatusService
{
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;

    public LessonStatusService(ApplicationDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<IEnumerable<LessonStatusDto>> GetAllAsync() =>
        _mapper.Map<IEnumerable<LessonStatusDto>>(await _db.LessonStatuses.ToListAsync());

    public async Task<LessonStatusDto?> GetByIdAsync(int id)
    {
        var e = await _db.LessonStatuses.FindAsync(id);
        return e == null ? null : _mapper.Map<LessonStatusDto>(e);
    }

    public async Task<LessonStatusDto> CreateAsync(CreateLessonStatusDto dto)
    {
        var e = _mapper.Map<LessonStatus>(dto);
        _db.LessonStatuses.Add(e);
        await _db.SaveChangesAsync();
        return _mapper.Map<LessonStatusDto>(e);
    }

    public async Task UpdateAsync(int id, UpdateLessonStatusDto dto)
    {
        var e = await _db.LessonStatuses.FindAsync(id);
        if (e == null) return;
        _mapper.Map(dto, e);
        await _db.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var e = await _db.LessonStatuses.FindAsync(id);
        if (e == null) return false;
        _db.LessonStatuses.Remove(e);
        await _db.SaveChangesAsync();
        return true;
    }
}