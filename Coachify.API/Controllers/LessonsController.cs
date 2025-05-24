using Coachify.BLL.DTOs.Lesson;
using Coachify.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Coachify.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LessonsController : ControllerBase
    {
        private readonly ILessonService _service;

        public LessonsController(ILessonService service)
        {
            _service = service;
        }

        // GET: api/lessons
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var lessons = await _service.GetAllAsync();
            return Ok(lessons);
        }

        // GET: api/lessons/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var lesson = await _service.GetByIdAsync(id);
            if (lesson == null)
                return NotFound();
            return Ok(lesson);
        }

        // POST: api/lessons
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateLessonDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var createdLesson = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(Get), new { id = createdLesson.LessonId }, createdLesson);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/lessons/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateLessonDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updatedLesson = await _service.UpdateAsync(id, dto);
                return Ok(updatedLesson);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE: api/lessons/5
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
                return BadRequest(new { message = ex.Message });
            }
        }

        // POST: api/lessons/start/10/5
        [HttpPost("start/{userId}/{lessonId}")]
        public async Task<IActionResult> StartLesson(int userId, int lessonId)
        {
            try
            {
                var result = await _service.StartLessonAsync(userId, lessonId);
                return Ok(new { success = result });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Произошла ошибка при начале прохождения урока" });
            }
        }

        // POST: api/lessons/complete/10/5
        [HttpPost("complete/{userId}/{lessonId}")]
        public async Task<IActionResult> CompleteLesson(int userId, int lessonId)
        {
            try
            {
                var result = await _service.CompleteLessonAsync(userId, lessonId);
                return Ok(new { success = result });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Произошла ошибка при завершении урока" });
            }
        }
    }
}
