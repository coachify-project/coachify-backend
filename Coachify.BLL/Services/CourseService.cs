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
        var course = _mapper.Map<Course>(dto);

        // Находим статус Draft
        var draftStatus = await _db.CourseStatuses
            .FirstOrDefaultAsync(s => s.Name == "Draft"); // имя зависит от того, как ты назвал

        if (draftStatus == null)
            throw new Exception("Draft status not found in database");

        course.StatusId = draftStatus.StatusId;

        _db.Courses.Add(course);
        await _db.SaveChangesAsync();

        return _mapper.Map<CourseDto>(course);
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
    public async Task<bool> SubmitCourseAsync(int courseId)
    {
        var course = await _db.Courses.FindAsync(courseId);
        if (course == null || course.StatusId != 1) // только если статус Draft
            return false;

        course.StatusId = 2; // Pending
        await _db.SaveChangesAsync();
        return true;
    }
    
    public async Task<bool> ApproveCourseAsync(int courseId)
    {
        var course = await _db.Courses.FindAsync(courseId);
        if (course == null || course.StatusId != 2) // Только если статус Pending
            return false;

        course.StatusId = 3; // Published
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RejectCourseAsync(int courseId)
    {
        var course = await _db.Courses.FindAsync(courseId);
        if (course == null || course.StatusId != 2) // Только если статус Pending
            return false;

        course.StatusId = 4; // Rejected
        await _db.SaveChangesAsync();
        return true;
    }

    
    public async Task<bool> StartCourseAsync(int courseId, int userId)
    {
        var enrollment = await _db.Enrollments
            .FirstOrDefaultAsync(e => e.CourseId == courseId && e.UserId == userId);

        if (enrollment == null || enrollment.StatusId != 1) // Not Started
            return false;

        enrollment.StatusId = 2; // In progress
        await _db.SaveChangesAsync();
        return true;
    }
    public async Task<bool> CompleteCourseAsync(int courseId, int userId)
    {
        var enrollment = await _db.Enrollments
            .FirstOrDefaultAsync(e => e.CourseId == courseId && e.UserId == userId);

        if (enrollment == null || enrollment.StatusId != 6) // In progress
            return false;

        enrollment.StatusId = 7; // Completed
        await _db.SaveChangesAsync();
        return true;
    }

    
    
}