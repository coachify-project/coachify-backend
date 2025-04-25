using Coachify.DAL;
using Coachify.DAL.Entities;
using Coachify.BLL.DTO;
using Microsoft.EntityFrameworkCore;

namespace Coachify.BLL.Services;

public class CourseService
{
    private readonly ApplicationDbContext _context;

    public CourseService(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<CourseDTO> GetAllCourses()
    {
        var courses = _context.Course
            .Select(c => new CourseDTO
            {
                CourseId = c.CourseId,
                Title = c.Title
            })
            .ToList();

        return courses;
    }
}