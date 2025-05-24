using System.Threading.Tasks;
using Coachify.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> GetCompletedLessons(int userId, int courseId)
        {
            var list = await _progress.GetCompletedLessonsAsync(userId, courseId);
            return Ok(new { completedLessons = list });
        }

        // GET api/progress/user/5/course/10/modules
        [HttpGet("user/{userId}/course/{courseId}/modules")]
        public async Task<IActionResult> GetCompletedModules(int userId, int courseId)
        {
            var list = await _progress.GetCompletedModulesAsync(userId, courseId);
            return Ok(new { completedModules = list });
        }

        // POST api/progress/lessons/start/5/20
        [HttpPost("lessons/start/{userId}/{lessonId}")]
        public async Task<IActionResult> StartLesson(int userId, int lessonId)
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
        public async Task<IActionResult> CompleteLesson(int userId, int lessonId)
        {
            var result = await _progress.CompleteLessonAsync(userId, lessonId);
            return Ok(new { success = result });
        }

        // POST api/progress/modules/start/5/7
        [HttpPost("modules/start/{userId}/{moduleId}")]
        public async Task<IActionResult> StartModule(int userId, int moduleId)
        {
            var result = await _progress.StartModuleAsync(userId, moduleId);
            return Ok(new { success = result });
        }

        // POST api/progress/modules/complete/5/7
        [HttpPost("modules/complete/{userId}/{moduleId}")]
        public async Task<IActionResult> CompleteModule(int userId, int moduleId)
        {
            var result = await _progress.CompleteModuleAsync(userId, moduleId);
            return Ok(new { success = result });
        }
    }
}