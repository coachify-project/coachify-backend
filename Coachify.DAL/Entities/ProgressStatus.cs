using System.ComponentModel.DataAnnotations;

namespace Coachify.DAL.Entities
{
    public class ProgressStatus
    {
        [Key]
        public int StatusId { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; } = null!;
        
        public ICollection<UserLessonProgress> LessonProgresses { get; set; } = new List<UserLessonProgress>();
        public ICollection<UserModuleProgress> ModuleProgresses { get; set; } = new List<UserModuleProgress>();

    }
}