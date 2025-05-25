// Lesson.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coachify.DAL.Entities
{
    public class Lesson
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LessonId { get; set; }

        [Required, MaxLength(255)]
        public string Title { get; set; } = null!;

        [MaxLength(255)]
        public string? Introduction { get; set; }

        [MaxLength(255)]
        public string? LessonObjectives { get; set; }

        [MaxLength(255)]
        public string VideoUrl { get; set; }

        public int ModuleId { get; set; }
        public Module Module { get; set; } = null!;
        public int StatusId { get; set; }
        public ProgressStatus Status { get; set; } = null!;
        
        public ICollection<UserLessonProgress> UserProgresses { get; set; } = new List<UserLessonProgress>();
    }
}