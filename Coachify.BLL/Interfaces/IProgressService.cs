
namespace Coachify.BLL.Interfaces;

public interface IProgressService
{
    Task<bool> StartLessonAsync(int userId, int lessonId);
    Task<bool> CompleteLessonAsync(int userId, int lessonId);
    Task<bool> StartModuleAsync(int userId, int moduleId);
    Task<bool> CompleteModuleAsync(int userId, int moduleId);
    Task<IEnumerable<int>> GetCompletedLessonsAsync(int userId, int courseId);
    Task<IEnumerable<int>> GetCompletedModulesAsync(int userId, int courseId);

}