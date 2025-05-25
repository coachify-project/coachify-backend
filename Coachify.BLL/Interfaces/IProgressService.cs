// IProgressService.cs (обновленный)
using System.Collections.Generic;
using System.Threading.Tasks;
using Coachify.DAL.Entities;

namespace Coachify.BLL.Interfaces
{
    public interface IProgressService
    {
        Task<IEnumerable<int>> GetCompletedLessonsAsync(int userId, int courseId);
        Task<IEnumerable<int>> GetCompletedModulesAsync(int userId, int courseId);
        Task<IEnumerable<UserLessonProgress>> GetUserLessonProgressAsync(int userId, int moduleId);
        Task<bool> StartLessonAsync(int userId, int lessonId);
        Task<bool> CompleteLessonAsync(int userId, int lessonId);
        Task<bool> StartModuleAsync(int userId, int moduleId);
        Task<bool> CompleteModuleAsync(int userId, int moduleId);
    }
}