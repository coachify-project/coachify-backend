// Module.cs

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coachify.DAL.Entities
{
    public class Module
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ModuleId { get; set; }

        [Required, MaxLength(255)] public string Title { get; set; } = null!;

        [Required, MaxLength(255)] public string Description { get; set; } = null!;

        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;

        // убраны поля StatusId и Status

        public ICollection<Skill> Skills { get; set; } = new List<Skill>();
        public ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();

        public int? TestId { get; set; }
        public Test? Test { get; set; }
        public int StatusId { get; set; }
        public ProgressStatus Status { get; set; } = null!;

        // Навигация на прогресс
        public ICollection<UserModuleProgress> UserProgresses { get; set; } = new List<UserModuleProgress>();
    }
}