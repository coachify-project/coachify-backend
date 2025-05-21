using AutoMapper;
using Coachify.BLL.DTOs.CourseStatus;
using Coachify.BLL.Interfaces;
using Coachify.DAL;
using Coachify.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Coachify.BLL.Services;

public class CourseStatusService : ICourseStatusService
{
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;

    public CourseStatusService(ApplicationDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CourseStatusDto>> GetAllAsync() =>
        _mapper.Map<IEnumerable<CourseStatusDto>>(await _db.CourseStatuses.ToListAsync());

    public async Task<CourseStatusDto?> GetByIdAsync(int id)
    {
        var e = await _db.CourseStatuses.FindAsync(id);
        return e == null ? null : _mapper.Map<CourseStatusDto>(e);
    }

    public async Task<CourseStatusDto> CreateAsync(CreateCourseStatusDto dto)
    {
        var e = _mapper.Map<CourseStatus>(dto);
        _db.CourseStatuses.Add(e);
        await _db.SaveChangesAsync();
        return _mapper.Map<CourseStatusDto>(e);
    }

    public async Task UpdateAsync(int id, UpdateCourseStatusDto dto)
    {
        var e = await _db.CourseStatuses.FindAsync(id);
        if (e == null) return;
        _mapper.Map(dto, e);
        await _db.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var e = await _db.CourseStatuses.FindAsync(id);
        if (e == null) return false;
        _db.CourseStatuses.Remove(e);
        await _db.SaveChangesAsync();
        return true;
    }
}