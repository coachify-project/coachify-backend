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
            .FirstOrDefaultAsync(c => c.CourseId == id);

        if (course == null)
            return null;

        var courseDto = _mapper.Map<CourseDto>(course);
        return courseDto;
    }
    
    public async Task<IEnumerable<CourseDto>> GetCoursesForAdminReviewAsync()
    {
        var courses = await _db.Courses
            .OrderBy(c => c.StatusId == 2 ? 0   // Pending → позиция 0
                : c.StatusId == 3 ? 1   // Published → 1
                : c.StatusId == 4 ? 2   // Rejected  → 2
                : 3)                    // остальные → 3
            .ThenBy(c => c.Title)                // вторичная сортировка по названию
            .ToListAsync();

        return _mapper.Map<IEnumerable<CourseDto>>(courses);
    }

    
    public async Task<IEnumerable<CourseDto>> GetCoursesByRoleIdAsync(int roleId)
    {
        IQueryable<Course> query = _db.Courses;

        if (roleId == 2 || roleId == 3) // User или Coach
        {
            query = query.Where(c => c.StatusId == 3); // Только опубликованные
        }
        else if (roleId == 1) // Admin
        {
            query = query.Where(c => c.StatusId == 2 || c.StatusId == 3); // Pending + Published
        }

        var courses = await query.ToListAsync();
        return _mapper.Map<IEnumerable<CourseDto>>(courses);
    }

    public async Task<IEnumerable<CourseDto>> GetCoachCoursesAsync(int  coachId)
    {
        var courses = await _db.Courses
            .Where(c => c.CoachId == coachId) // важно: CoachId — это Id создателя курса
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
                CourseId          = e.Course.CourseId,
                Title             = e.Course.Title,
                CoachId           = e.Course.CoachId,
                CategoryId        = e.Course.CategoryId,
                EnrollmentStatusId= e.StatusId
            })
            .ToListAsync();

        return list;
    }


    public async Task<CourseDto> CreateAsync(CreateCourseDto dto)
    {
        //var course = _mapper.Map<Course>(dto);

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
            StatusId = draftStatus.StatusId
        };
        
        //course.StatusId = draftStatus.StatusId;

        _db.Courses.Add(course);
        await _db.SaveChangesAsync();

        //return _mapper.Map<CourseDto>(course);
        return new CourseDto
        {
            CourseId = course.CourseId,
            Title = course.Title,
            Description = course.Description,
            Price = course.Price,
            MaxClients = course.MaxClients,
            PosterUrl = course.PosterUrl,
            CategoryId = course.CategoryId,
            CoachId = course.CoachId,
        };
    }


    public async Task<CourseDto> UpdateAsync(int id, UpdateCourseDto dto)
    {
        var course = await _db.Courses.FindAsync(id);
        if (course == null)
            throw new KeyNotFoundException($"Course {id} not found");

        // Только Draft или Rejected
        if (course.StatusId != 1 && course.StatusId != 4)
            throw new InvalidOperationException("Редактировать можно только черновик или отклонённый курс.");

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
        
        //_mapper.Map(dto, course);
        await _db.SaveChangesAsync();

        return new CourseDto
        {
            CourseId = course.CourseId,
            Title = course.Title,
            Description = course.Description,
            Price = course.Price,
            MaxClients = course.MaxClients,
            PosterUrl = course.PosterUrl,           
            CategoryId = course.CategoryId,
            CoachId = course.CoachId,
        };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var course = await _db.Courses.FindAsync(id);
        if (course == null) return false;

        if (course.StatusId != 1 && course.StatusId != 4)
            throw new InvalidOperationException("Удалить можно только черновик или отклонённый курс.");

        _db.Courses.Remove(course);
        await _db.SaveChangesAsync();
        return true;
    }
    public async Task<bool> SubmitCourseAsync(int courseId, int coachId)
    {
        var course = await _db.Courses
            .FirstOrDefaultAsync(c => c.CourseId == courseId && c.CoachId == coachId);

        if (course == null || course.StatusId != 1) //draft
            return false;

        course.StatusId = 2; //pending
        await _db.SaveChangesAsync();
        return true;
    }

    
    public async Task<bool> ApproveCourseAsync(int courseId)
    {
        var course = await _db.Courses.FindAsync(courseId);
        if (course == null || course.StatusId != 2) //pending
            return false;

        course.StatusId = 3; //published
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RejectCourseAsync(int courseId)
    {
        var course = await _db.Courses.FindAsync(courseId);
        if (course == null || course.StatusId != 2) // pending
            return false;

        course.StatusId = 4; // rejected
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