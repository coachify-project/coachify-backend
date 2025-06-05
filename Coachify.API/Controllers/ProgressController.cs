// ProgressController.cs (исправленная версия)
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Coachify.BLL.Interfaces;

namespace Coachify.API.Controllers
{
    [ApiController]
    [Route("api/progress")]
    public class ProgressController : ControllerBase
    {
        private readonly IProgressService _progress;

        public ProgressController(IProgressService progress)
        {
            _progress = progress;
        }

        // GET api/progress/user/5/course/10/lessons
        [HttpGet("user/{userId}/course/{courseId}/lessons")]
        public async Task<ActionResult> GetCompletedLessons(int userId, int courseId)
        {
            try
            {
                var list = await _progress.GetCompletedLessonsAsync(userId, courseId);
                return Ok(new { completedLessons = list });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // GET api/progress/user/5/course/10/modules
        [HttpGet("user/{userId}/course/{courseId}/modules")]
        public async Task<ActionResult> GetCompletedModules(int userId, int courseId)
        {
            try
            {
                var list = await _progress.GetCompletedModulesAsync(userId, courseId);
                return Ok(new { completedModules = list });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // GET api/progress/user/5/module/10/lessons - НОВЫЙ МЕТОД
        [HttpGet("user/{userId}/module/{moduleId}/lessons")]
        public async Task<ActionResult> GetUserLessonProgress(int userId, int moduleId)
        {
            try
            {
                var progress = await _progress.GetUserLessonProgressAsync(userId, moduleId);
                return Ok(new { lessonProgress = progress });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // POST api/progress/lessons/start/5/20
        [HttpPost("lessons/start/{userId}/{lessonId}")]
        public async Task<ActionResult> StartLesson(int userId, int lessonId)
        {
            try
            {
                var result = await _progress.StartLessonAsync(userId, lessonId);
                return Ok(new { success = result });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // POST api/progress/lessons/complete/5/20
        [HttpPost("lessons/complete/{userId}/{lessonId}")]
        public async Task<ActionResult> CompleteLesson(int userId, int lessonId)
        {
            try
            {
                var result = await _progress.CompleteLessonAsync(userId, lessonId);
                return Ok(new { success = result });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // POST api/progress/modules/start/5/7
        [HttpPost("modules/start/{userId}/{moduleId}")]
        public async Task<ActionResult> StartModule(int userId, int moduleId)
        {
            try
            {
                var result = await _progress.StartModuleAsync(userId, moduleId);
                return Ok(new { success = result });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // POST api/progress/modules/complete/5/7
        [HttpPost("modules/complete/{userId}/{moduleId}")]
        public async Task<ActionResult> CompleteModule(int userId, int moduleId)
        {
            try
            {
                var result = await _progress.CompleteModuleAsync(userId, moduleId);
                return Ok(new { success = result });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}