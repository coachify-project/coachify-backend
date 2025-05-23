using Coachify.BLL.DTOs.Enrollment;
using Coachify.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Coachify.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnrollmentsController : ControllerBase
    {
        private readonly IEnrollmentService _enrollmentService;

        public EnrollmentsController(IEnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }

        // GET: api/enrollments
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var enrollments = await _enrollmentService.GetAllAsync();
            return Ok(enrollments);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var enrollment = await _enrollmentService.GetByIdAsync(id);
            if (enrollment == null)
                return NotFound();
            return Ok(enrollment);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateEnrollmentDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var created = await _enrollmentService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.EnrollmentId }, created);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // PUT: api/enrollments/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateEnrollmentDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _enrollmentService.UpdateAsync(id, dto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // DELETE: api/enrollments/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _enrollmentService.DeleteAsync(id);
            if (!deleted)
                return NotFound();
            return NoContent();
        }


        // POST: api/enrollments/start
        [HttpPost("user/start-course")]
        public async Task<IActionResult> StartCourse([FromQuery] int courseId, [FromQuery] int userId)
        {
            var enrollment = await _enrollmentService.StartCourseAsync(courseId, userId);
            return Ok(enrollment);
        }

        // POST: api/enrollments/complete/{enrollmentId}
        [HttpPost("user/complete/{enrollmentId}")]
        public async Task<IActionResult> CompleteEnrollment(int enrollmentId)
        {
            try
            {
                await _enrollmentService.CompleteEnrollmentAsync(enrollmentId);
                return Ok("Enrollment marked as completed and certificate issued.");
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // POST: api/enrollments/complete-course
        [HttpPost("user/complete-course")]
        public async Task<IActionResult> CompleteCourse([FromQuery] int courseId, [FromQuery] int userId)
        {
            var success = await _enrollmentService.CompleteCourseAsync(courseId, userId);
            if (!success)
                return NotFound();

            return Ok("Course completed and certificate issued.");
        }
    }
}