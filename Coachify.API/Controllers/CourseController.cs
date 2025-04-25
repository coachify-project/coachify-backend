using Coachify.BLL.Services;
using Coachify.BLL.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Coachify.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CoursesController : ControllerBase
{
    private readonly CourseService _courseService;

    public CoursesController(CourseService courseService)
    {
        _courseService = courseService;
    }

    [HttpGet]
    public ActionResult<List<CourseDTO>> GetCourses()
    {
        var courses = _courseService.GetAllCourses();
        return Ok(courses);
    }
}