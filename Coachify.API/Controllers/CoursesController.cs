using Coachify.BLL.DTOs.Course;
using Coachify.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Coachify.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly ICourseService _service;
    public CoursesController(ICourseService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var course = await _service.GetByIdAsync(id);
        return course == null ? NotFound() : Ok(course);
    }
    
    [HttpGet("user/courses/{userId}")]
    public async Task<IActionResult> GetByUser(int userId)
    {
        var courses = await _service.GetCoursesByUserAsync(userId);
        return Ok(courses);
    }

    [HttpGet("admin/courses")]
    public async Task<IActionResult> GetForAdminReview()
    {
        var list = await _service.GetCoursesForAdminReviewAsync();
        return Ok(list);
    }

    [HttpGet("by-role/courses/{roleId}")]
    public async Task<IActionResult> GetCoursesByRole(int roleId)
    {
        var courses = await _service.GetCoursesByRoleIdAsync(roleId);
        return Ok(courses);
    }

    [HttpGet("coach/courses/{coachId}")]
    public async Task<IActionResult> GetCoachCourses(int coachId)
    {
        var courses = await _service.GetCoachCoursesAsync(coachId);
        return Ok(courses);
    }
    
    [HttpGet("coach/{coachId}/published-courses")]
    public async Task<IActionResult> GetPublishedCoursesByCoach(int coachId)
    {
        var courses = await _service.GetPublishedCoursesByCoachIdAsync(coachId);
        return Ok(courses);
    }

    [HttpPost("coach/create-course")]
    public async Task<IActionResult> Create([FromBody] CreateCourseDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var course = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(Get), new { id = course.CourseId }, course);
    }

    [HttpPut("coach/update-course/{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCourseDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        try
        {
            var updatedCourse = await _service.UpdateAsync(id, dto);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted)
                return NotFound();
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("coach/submit")]
    public async Task<IActionResult> Submit(int courseId, int coachId)
    {
        try
        {
            var result = await _service.SubmitCourseAsync(courseId, coachId);
            return result ? Ok() : BadRequest("Course not in Draft status.");
        }
        catch (KeyNotFoundException)
        {
            return NotFound("Course not found or not owned by the coach.");
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("admin/approve/{courseId}")]
    public async Task<IActionResult> Approve(int courseId)
    {
        try
        {
            var result = await _service.ApproveCourseAsync(courseId);
            return result ? Ok() : BadRequest("Course not in Pending status.");
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("admin/reject/{courseId}")]
    public async Task<IActionResult> Reject(int courseId)
    {
        try
        {
            var result = await _service.RejectCourseAsync(courseId);
            return result ? Ok() : BadRequest("Course not in Pending status.");
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
