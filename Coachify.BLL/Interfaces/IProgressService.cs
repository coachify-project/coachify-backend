using System.Collections.Generic;
using System.Threading.Tasks;
using Coachify.DAL.Entities;

namespace Coachify.BLL.Interfaces
{
    public interface IProgressService
    {
        // Существующие методы
        Task<IEnumerable<UserLessonProgress>> GetUserLessonProgressAsync(int userId, int moduleId);
        Task<bool> StartLessonAsync(int userId, int lessonId);
        Task<bool> CompleteLessonAsync(int userId, int lessonId);
        Task<bool> StartModuleAsync(int userId, int moduleId);
        Task<bool> CompleteModuleAsync(int userId, int moduleId);
        Task<IEnumerable<int>> GetCompletedLessonsAsync(int userId, int courseId);
        Task<IEnumerable<int>> GetCompletedModulesAsync(int userId, int courseId);

        Task<bool> StartCourseAsync(int userId, int courseId);
        Task<int> GetLessonProgressStatusAsync(int userId, int lessonId);
        Task<int> GetModuleProgressStatusAsync(int userId, int moduleId);
        Task<int> GetCourseProgressStatusAsync(int userId, int courseId);
        Task<int> GetCourseProgressPercentageAsync(int userId, int courseId);
        Task<Dictionary<int, int>> GetModuleLessonsProgressStatusAsync(int userId, int moduleId);
        Task<Dictionary<int, int>> GetCourseModulesProgressStatusAsync(int userId, int courseId);
    }
}