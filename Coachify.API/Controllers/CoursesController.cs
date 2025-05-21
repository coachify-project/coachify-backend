using Coachify.BLL.DTOs.Course;
using Coachify.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
        var d = await _service.GetByIdAsync(id);
        return d == null ? NotFound() : Ok(d);
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


    [HttpPost("coach/create-course")]
    public async Task<IActionResult> Create(CreateCourseDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var c = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(Get), new { id = c.CourseId }, c);
    }

    [HttpPut("coach/update-course/{id}")]
    public async Task<IActionResult> Update(int id, UpdateCourseDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        await _service.UpdateAsync(id, dto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id) => Ok(await _service.DeleteAsync(id));


    [HttpPost("coach/submit")]
    public async Task<IActionResult> Submit(int courseId, int coachId)
    {
        var result = await _service.SubmitCourseAsync(courseId, coachId);
        return result ? Ok() : BadRequest("Course not in Draft status.");
    }

    [HttpPost("admin/approve/{courseId}")]
    public async Task<IActionResult> Approve(int courseId)
    {
        var result = await _service.ApproveCourseAsync(courseId);
        return result ? Ok() : BadRequest("Course not in Pending status.");
    }

    [HttpPost("admin/reject/{courseId}")]
    public async Task<IActionResult> Reject(int courseId)
    {
        var result = await _service.RejectCourseAsync(courseId);
        return result ? Ok() : BadRequest("Course not in Pending status.");
    }
}