// ProgressController.cs (обновленная версия)
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

        // Существующие методы

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

        // GET api/progress/user/5/module/10/lessons
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

        // НОВЫЙ МЕТОД ДЛЯ НАЧАЛА КУРСА
        // POST api/progress/courses/start/5/10
        [HttpPost("courses/start/{userId}/{courseId}")]
        public async Task<ActionResult> StartCourse(int userId, int courseId)
        {
            try
            {
                var result = await _progress.StartCourseAsync(userId, courseId);
                return Ok(new { success = result });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // МЕТОДЫ ДЛЯ ПОЛУЧЕНИЯ СТАТУСА ПРОГРЕССА

        // GET api/progress/user/5/lesson/20/status
        [HttpGet("user/{userId}/lesson/{lessonId}/status")]
        public async Task<ActionResult> GetLessonProgressStatus(int userId, int lessonId)
        {
            try
            {
                var status = await _progress.GetLessonProgressStatusAsync(userId, lessonId);
                return Ok(new { statusId = status });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // GET api/progress/user/5/module/7/status
        [HttpGet("user/{userId}/module/{moduleId}/status")]
        public async Task<ActionResult> GetModuleProgressStatus(int userId, int moduleId)
        {
            try
            {
                var status = await _progress.GetModuleProgressStatusAsync(userId, moduleId);
                return Ok(new { statusId = status });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // GET api/progress/user/5/course/10/status
        [HttpGet("user/{userId}/course/{courseId}/status")]
        public async Task<ActionResult> GetCourseProgressStatus(int userId, int courseId)
        {
            try
            {
                var status = await _progress.GetCourseProgressStatusAsync(userId, courseId);
                return Ok(new { statusId = status });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // GET api/progress/user/5/course/10/percentage
        [HttpGet("user/{userId}/course/{courseId}/percentage")]
        public async Task<ActionResult> GetCourseProgressPercentage(int userId, int courseId)
        {
            try
            {
                var percentage = await _progress.GetCourseProgressPercentageAsync(userId, courseId);
                return Ok(new { progressPercentage = percentage });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // GET api/progress/user/5/module/7/lessons/status
        [HttpGet("user/{userId}/module/{moduleId}/lessons/status")]
        public async Task<ActionResult> GetModuleLessonsProgressStatus(int userId, int moduleId)
        {
            try
            {
                var statuses = await _progress.GetModuleLessonsProgressStatusAsync(userId, moduleId);
                return Ok(new { lessonStatuses = statuses });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // GET api/progress/user/5/course/10/modules/status
        [HttpGet("user/{userId}/course/{courseId}/modules/status")]
        public async Task<ActionResult> GetCourseModulesProgressStatus(int userId, int courseId)
        {
            try
            {
                var statuses = await _progress.GetCourseModulesProgressStatusAsync(userId, courseId);
                return Ok(new { moduleStatuses = statuses });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // GET api/progress/user/5/course/10/overview - Комбинированный метод для получения полного обзора прогресса
        [HttpGet("user/{userId}/course/{courseId}/overview")]
        public async Task<ActionResult> GetCourseProgressOverview(int userId, int courseId)
        {
            try
            {
                var courseStatus = await _progress.GetCourseProgressStatusAsync(userId, courseId);
                var coursePercentage = await _progress.GetCourseProgressPercentageAsync(userId, courseId);
                var moduleStatuses = await _progress.GetCourseModulesProgressStatusAsync(userId, courseId);
                var completedLessons = await _progress.GetCompletedLessonsAsync(userId, courseId);
                var completedModules = await _progress.GetCompletedModulesAsync(userId, courseId);

                return Ok(new 
                { 
                    courseStatusId = courseStatus,
                    progressPercentage = coursePercentage,
                    moduleStatuses = moduleStatuses,
                    completedLessons = completedLessons,
                    completedModules = completedModules
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // GET api/progress/user/5/module/7/overview - Комбинированный метод для получения полного обзора прогресса модуля
        [HttpGet("user/{userId}/module/{moduleId}/overview")]
        public async Task<ActionResult> GetModuleProgressOverview(int userId, int moduleId)
        {
            try
            {
                var moduleStatus = await _progress.GetModuleProgressStatusAsync(userId, moduleId);
                var lessonStatuses = await _progress.GetModuleLessonsProgressStatusAsync(userId, moduleId);
                var lessonProgress = await _progress.GetUserLessonProgressAsync(userId, moduleId);

                return Ok(new 
                { 
                    moduleStatusId = moduleStatus,
                    lessonStatuses = lessonStatuses,
                    lessonProgress = lessonProgress
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}