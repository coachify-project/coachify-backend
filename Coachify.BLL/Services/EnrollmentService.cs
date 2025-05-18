using Coachify.BLL.DTOs.Enrollment;
using Coachify.BLL.Interfaces;
using Coachify.DAL;
using Coachify.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class EnrollmentService : IEnrollmentService
{
    private readonly ApplicationDbContext _db;
    private readonly ICertificateService _certificateService;

    public EnrollmentService(ApplicationDbContext db, ICertificateService certificateService)
    {
        _db = db;
        _certificateService = certificateService;
    }

    public async Task<IEnumerable<EnrollmentDto>> GetAllAsync()
    {
        var enrollments = await _db.Enrollments
            .Include(e => e.User)
            .Include(e => e.Course)
            .ToListAsync();

        return enrollments.Select(e => new EnrollmentDto
        {
            EnrollmentId = e.EnrollmentId,
            UserId = e.UserId,
            CourseId = e.CourseId,
            StatusId = e.StatusId,
            FirstName = e.User.FirstName,
            LastName = e.User.LastName,
            CourseTitle = e.Course.Title,
            EnrolledAt = e.EnrolledAt
        });
    }

    public async Task<EnrollmentDto?> GetByIdAsync(int id)
    {
        var e = await _db.Enrollments
            .Include(x => x.User)
            .Include(x => x.Course)
            .FirstOrDefaultAsync(x => x.EnrollmentId == id);

        if (e == null) return null;

        return new EnrollmentDto
        {
            EnrollmentId = e.EnrollmentId,
            UserId = e.UserId,
            CourseId = e.CourseId,
            StatusId = e.StatusId,
            FirstName = e.User.FirstName,
            LastName = e.User.LastName,
            CourseTitle = e.Course.Title,
            EnrolledAt = e.EnrolledAt
        };
    }

    public async Task<EnrollmentDto> CreateAsync(CreateEnrollmentDto dto)
    {
        var course = await _db.Courses.FindAsync(dto.CourseId);
        if (course == null || course.StatusId != 3) // Published
            throw new Exception("Курс недоступен для записи");

        var enrollment = new Enrollment
        {
            UserId = dto.UserId,
            CourseId = dto.CourseId,
            StatusId = 1,
            EnrolledAt = System.DateTime.UtcNow
        };

        _db.Enrollments.Add(enrollment);
        await _db.SaveChangesAsync();

        return await GetByIdAsync(enrollment.EnrollmentId) ?? throw new System.Exception("Ошибка создания записи");
    }

    public async Task UpdateAsync(int id, UpdateEnrollmentDto dto)
    {
        var enrollment = await _db.Enrollments.FindAsync(id);
        if (enrollment == null)
            throw new KeyNotFoundException($"Enrollment with id={id} not found");

        enrollment.StatusId = dto.StatusId;
        // другие поля если надо

        await _db.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var enrollment = await _db.Enrollments.FindAsync(id);
        if (enrollment == null) return false;

        _db.Enrollments.Remove(enrollment);
        await _db.SaveChangesAsync();

        return true;
    }

    public async Task<bool> EnrollUserAsync(int courseId, int userId)
    {
        var course = await _db.Courses.FindAsync(courseId);

        if (course == null)
            throw new Exception("Курс не найден.");

        if (course.StatusId != 3) // 3 = Published
            throw new Exception("Курс ещё не опубликован и недоступен для записи.");

        bool exists = await _db.Enrollments.AnyAsync(e => e.CourseId == courseId && e.UserId == userId);
        if (exists)
            throw new Exception("Пользователь уже записан на этот курс.");

        var enrollment = new Enrollment
        {
            CourseId = courseId,
            UserId = userId,
            StatusId = 1, // Registered
            EnrolledAt = DateTime.UtcNow
        };

        _db.Enrollments.Add(enrollment);
        await _db.SaveChangesAsync();

        return true;
    }

    
    public async Task<EnrollmentDto> StartCourseAsync(int courseId, int userId)
    {
        var course = await _db.Courses.FindAsync(courseId);
        if (course == null || course.StatusId != 3) // 3 = Published
            throw new Exception("Курс недоступен для начала");

        var enrollment = await _db.Enrollments
            .FirstOrDefaultAsync(e => e.CourseId == courseId && e.UserId == userId);

        if (enrollment == null)
        {
            enrollment = new Enrollment
            {
                CourseId = courseId,
                UserId = userId,
                StatusId = 2, // In Progress
                EnrolledAt = System.DateTime.UtcNow
            };
            _db.Enrollments.Add(enrollment);
        }
        else
        {
            enrollment.StatusId = 2; // In Progress
        }

        await _db.SaveChangesAsync();
        return await GetByIdAsync(enrollment.EnrollmentId) ?? throw new System.Exception("Ошибка при старте курса");
    }

    public async Task CompleteEnrollmentAsync(int enrollmentId)
    {
        var enrollment = await _db.Enrollments.FindAsync(enrollmentId);
        if (enrollment == null) 
            throw new KeyNotFoundException($"Enrollment with id={enrollmentId} not found");

        enrollment.StatusId = 3; // Completed
        await _db.SaveChangesAsync();

        await _certificateService.CreateCertificateForEnrollmentAsync(enrollmentId);
    }

    public async Task<bool> CompleteCourseAsync(int courseId, int userId)
    {
        var enrollment = await _db.Enrollments
            .FirstOrDefaultAsync(e => e.CourseId == courseId && e.UserId == userId);

        if (enrollment == null)
            return false;

        enrollment.StatusId = 3; // Completed
        await _db.SaveChangesAsync();

        await _certificateService.CreateCertificateForEnrollmentAsync(enrollment.EnrollmentId);

        return true;
    }
}
