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

    public async Task<IEnumerable<CourseDto>> GetAllAsync()
    {
        var courses = await _db.Courses
            .Include(c => c.Modules)
            .ToListAsync();

        return _mapper.Map<IEnumerable<CourseDto>>(courses);
    }

    public async Task<CourseDto?> GetByIdAsync(int id)
    {
        var course = await _db.Courses
            .Include(c => c.Modules)
            .Include(c => c.Category)
            .Include(c => c.Coach)
            .Include(c => c.Status)
            .Include(c => c.Feedbacks)
            .FirstOrDefaultAsync(c => c.CourseId == id);

        if (course == null)
            return null;

        var courseDto = _mapper.Map<CourseDto>(course);
        return courseDto;
    }

    public async Task<IEnumerable<CourseDto>> GetCoursesForAdminReviewAsync()
    {
        var courses = await _db.Courses
            .Where(c => c.StatusId == 2 || c.StatusId == 3 || c.StatusId == 4) // фильтрация
            .OrderBy(c => c.StatusId == 2 ? 0 // Pending → 0
                : c.StatusId == 3 ? 1 // Published → 1
                : 2) // Rejected → 2
            .ThenBy(c => c.Title)
            .ToListAsync();

        return _mapper.Map<IEnumerable<CourseDto>>(courses);
    }


    public async Task<IEnumerable<CourseDto>> GetCoursesByRoleIdAsync(int roleId)
    {
        IQueryable<Course> query = _db.Courses;

        query = roleId switch
        {
            1 => query.Where(c => c.StatusId == 2 || c.StatusId == 3), // Admin
            2 or 3 => query.Where(c => c.StatusId == 3), // User/Coach
            _ => query.Where(c => false) // No access
        };


        var courses = await query.ToListAsync();
        return _mapper.Map<IEnumerable<CourseDto>>(courses);
    }

    public async Task<IEnumerable<CourseDto>> GetCoachCoursesAsync(int coachId)
    {
        var courses = await _db.Courses
            .Where(c => c.CoachId == coachId)
            .ToListAsync();

        return _mapper.Map<IEnumerable<CourseDto>>(courses);
    }

    public async Task<IEnumerable<CourseDto>> GetPublishedCoursesByCoachIdAsync(int coachId)
    {
        var courses = await _db.Courses
            .Where(c => c.CoachId == coachId && c.StatusId == 3) // Published
            .ToListAsync();

        return _mapper.Map<IEnumerable<CourseDto>>(courses);
    }


    public async Task<IEnumerable<UserCourseDto>> GetCoursesByUserAsync(int userId)
    {
        var list = await _db.Enrollments
            .Where(e => e.UserId == userId)
            .Include(e => e.Course)
            .Select(e => new UserCourseDto
            {
                CourseId = e.Course.CourseId,
                Title = e.Course.Title,
                CoachId = e.Course.CoachId,
                CategoryId = e.Course.CategoryId,
                PosterUrl = e.Course.PosterUrl,
                ProgressPercentage = e.ProgressPercentage,
                EnrollmentStatusId = e.StatusId
            })
            .ToListAsync();

        return list;
    }



    public async Task<CourseDto> CreateAsync(CreateCourseDto dto)
    {
        var category = await _db.Categories
            .FirstOrDefaultAsync(c => c.Name.ToLower() == dto.CategoryName.ToLower());

        if (category == null)
        {
            category = new Category { Name = dto.CategoryName };
            _db.Categories.Add(category);
            await _db.SaveChangesAsync();
        }

        var draftStatus = await _db.CourseStatuses
            .FirstOrDefaultAsync(s => s.StatusId == 1);

        if (draftStatus == null)
            throw new Exception("Draft status not found in database");

        var course = new Course
        {
            Title = dto.Title,
            Description = dto.Description,
            Price = dto.Price,
            MaxClients = dto.MaxClients,
            CategoryId = category.CategoryId,
            PosterUrl = dto.PosterUrl,
            CoachId = dto.CoachId,
            StatusId = draftStatus.StatusId,
            SubmittedAt = DateTime.UtcNow
        };

        _db.Courses.Add(course);
        await _db.SaveChangesAsync();

        return _mapper.Map<CourseDto>(course);
    }


    public async Task<CourseDto> UpdateAsync(int id, UpdateCourseDto dto)
    {
        var course = await _db.Courses.FindAsync(id);
        if (course == null)
            throw new KeyNotFoundException($"Course {id} not found");

        if (course.StatusId != 1 && course.StatusId != 4) // Draft or Rejected only
            throw new InvalidOperationException("Only draft or rejected courses can be edited.");

        var category = await _db.Categories
            .FirstOrDefaultAsync(c => c.Name.ToLower() == dto.CategoryName.ToLower());

        if (category == null)
        {
            category = new Category { Name = dto.CategoryName };
            _db.Categories.Add(category);
            await _db.SaveChangesAsync();
        }

        course.Title = dto.Title;
        course.Description = dto.Description;
        course.Price = dto.Price;
        course.MaxClients = dto.MaxClients;
        course.CategoryId = category.CategoryId;
        course.PosterUrl = dto.PosterUrl;

        await _db.SaveChangesAsync();

        return _mapper.Map<CourseDto>(course);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var course = await _db.Courses.FindAsync(id);
        if (course == null) return false;

        if (course.StatusId != 1 && course.StatusId != 4) // Draft or Rejected only
            throw new InvalidOperationException("Only draft or rejected courses can be deleted.");

        _db.Courses.Remove(course);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> SubmitCourseAsync(int courseId, int coachId)
    {
        var course = await _db.Courses
            .FirstOrDefaultAsync(c => c.CourseId == courseId && c.CoachId == coachId);

        if (course == null)
            throw new KeyNotFoundException("Course not found or not owned by the coach");

        if (course.StatusId != 1) // Draft only
            throw new InvalidOperationException("Only draft courses can be submitted.");

        course.StatusId = 2; // Pending
        course.SubmittedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return true;
    }


    public async Task<bool> ApproveCourseAsync(int courseId)
    {
        var course = await _db.Courses.FindAsync(courseId);
        if (course == null)
            throw new KeyNotFoundException("Course not found");

        if (course.StatusId != 2) // Pending only
            throw new InvalidOperationException("Only pending courses can be approved.");

        course.StatusId = 3; // Published
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RejectCourseAsync(int courseId)
    {
        var course = await _db.Courses.FindAsync(courseId);
        if (course == null)
            throw new KeyNotFoundException("Course not found");

        if (course.StatusId != 2) // Pending only
            throw new InvalidOperationException("Only pending courses can be rejected.");

        course.StatusId = 4; // Rejected
        await _db.SaveChangesAsync();
        return true;
    }


    public async Task<bool> StartCourseAsync(int courseId, int userId)
    {
        var enrollment = await _db.Enrollments
            .FirstOrDefaultAsync(e => e.CourseId == courseId && e.UserId == userId);

        if (enrollment == null || enrollment.StatusId != 1) // Not Started only
            return false;

        enrollment.StatusId = 2; // In Progress
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> CompleteCourseAsync(int courseId, int userId)
    {
        var enrollment = await _db.Enrollments
            .FirstOrDefaultAsync(e => e.CourseId == courseId && e.UserId == userId);

        if (enrollment == null || enrollment.StatusId != 2) // In Progress only
            return false;

        enrollment.StatusId = 3; // Completed
        await _db.SaveChangesAsync();
        return true;
    }
}