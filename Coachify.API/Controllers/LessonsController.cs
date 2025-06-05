// LessonsController.cs (исправленная версия)
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Coachify.BLL.Interfaces;
using Coachify.BLL.DTOs;
using Coachify.BLL.DTOs.Lesson;

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
        public async Task<ActionResult<IEnumerable<LessonDto>>> GetAll()
        {
            var lessons = await _service.GetAllAsync();
            return Ok(lessons);
        }

        // GET: api/lessons/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LessonDto>> Get(int id)
        {
            var lesson = await _service.GetByIdAsync(id);
            if (lesson == null)
                return NotFound();
            return Ok(lesson);
        }

        // GET: api/lessons/module/5
        [HttpGet("module/{moduleId}")]
        public async Task<ActionResult<IEnumerable<LessonDto>>> GetByModule(int moduleId)
        {
            try
            {
                var lessons = await _service.GetByModuleAsync(moduleId);
                return Ok(lessons);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: api/lessons/module/5/user/10
        [HttpGet("module/{moduleId}/user/{userId}")]
        public async Task<ActionResult<IEnumerable<LessonDto>>> GetByModuleForUser(int moduleId, int userId)
        {
            try
            {
                var lessons = await _service.GetByModuleForUserAsync(moduleId, userId);
                return Ok(lessons);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // POST: api/lessons
        [HttpPost]
        public async Task<ActionResult<LessonDto>> Create([FromBody] CreateLessonDto dto)
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
        public async Task<ActionResult<LessonDto>> Update(int id, [FromBody] UpdateLessonDto dto)
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
        public async Task<ActionResult> Delete(int id)
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
    }
}