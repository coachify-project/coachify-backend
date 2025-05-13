using AutoMapper;
using Coachify.BLL.DTOs.Enrollment;
using Coachify.BLL.Interfaces;
using Coachify.DAL;
using Coachify.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Coachify.BLL.Services;

public class EnrollmentService : IEnrollmentService
{
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;

    public EnrollmentService(ApplicationDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<IEnumerable<EnrollmentDto>> GetAllAsync() =>
        _mapper.Map<IEnumerable<EnrollmentDto>>(await _db.Enrollments.ToListAsync());

    public async Task<EnrollmentDto?> GetByIdAsync(int id)
    {
        var e = await _db.Enrollments.FindAsync(id);
        return e == null ? null : _mapper.Map<EnrollmentDto>(e);
    }

    public async Task<EnrollmentDto> CreateAsync(CreateEnrollmentDto dto)
    {
        var e = _mapper.Map<Enrollment>(dto);
        _db.Enrollments.Add(e);
        await _db.SaveChangesAsync();
        return _mapper.Map<EnrollmentDto>(e);
    }

    public async Task UpdateAsync(int id, UpdateEnrollmentDto dto)
    {
        var e = await _db.Enrollments.FindAsync(id);
        if (e == null) return;
        _mapper.Map(dto, e);
        await _db.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var e = await _db.Enrollments.FindAsync(id);
        if (e == null) return false;
        _db.Enrollments.Remove(e);
        await _db.SaveChangesAsync();
        return true;
    }


    public async Task<bool> EnrollUserAsync(int courseId, int userId)
    {
        var exists = await _db.Enrollments
            .AnyAsync(e => e.CourseId == courseId && e.UserId == userId);

        if (exists)
            return false;

        _db.Enrollments.Add(new Enrollment
        {
            CourseId = courseId,
            UserId = userId,
            StatusId = 1 // Not Started
        });

        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> StartCourseAsync(int courseId, int userId)
    {
        var enrollment = await _db.Enrollments
            .FirstOrDefaultAsync(e => e.CourseId == courseId && e.UserId == userId);

        if (enrollment == null || enrollment.StatusId != 1) // Not Started
            return false;

        enrollment.StatusId = 6; // In Progress
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> CompleteCourseAsync(int courseId, int userId)
    {
        var enrollment = await _db.Enrollments
            .FirstOrDefaultAsync(e => e.CourseId == courseId && e.UserId == userId);

        if (enrollment == null || enrollment.StatusId != 2) // In Progress
            return false;

        enrollment.StatusId = 3; // Completed
        await _db.SaveChangesAsync();
        return true;
    }
}