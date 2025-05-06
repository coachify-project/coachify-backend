using AutoMapper;
using Coachify.BLL.DTOs.Course;
using Coachify.BLL.Interfaces;
using Coachify.DAL;
using Coachify.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Coachify.BLL.Services;

public class CourseService : ICourseService
{
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;

    public CourseService(ApplicationDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CourseDto>> GetAllAsync() =>
        _mapper.Map<IEnumerable<CourseDto>>(await _db.Courses.ToListAsync());

    public async Task<CourseDto?> GetByIdAsync(int id)
    {
        var e = await _db.Courses.FindAsync(id);
        return e == null ? null : _mapper.Map<CourseDto>(e);
    }

    public async Task<CourseDto> CreateAsync(CreateCourseDto dto)
    {
        var e = _mapper.Map<Course>(dto);
        _db.Courses.Add(e);
        await _db.SaveChangesAsync();
        return _mapper.Map<CourseDto>(e);
    }

    public async Task UpdateAsync(int id, UpdateCourseDto dto)
    {
        var e = await _db.Courses.FindAsync(id);
        if (e == null) return;
        _mapper.Map(dto, e);
        await _db.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var e = await _db.Courses.FindAsync(id);
        if (e == null) return false;
        _db.Courses.Remove(e);
        await _db.SaveChangesAsync();
        return true;
    }
}