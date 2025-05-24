// UserModuleProgress.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace Coachify.DAL.Entities
{
    public class UserModuleProgress
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int ModuleId { get; set; }
        public Module Module { get; set; } = null!;

        [Required]
        public int StatusId { get; set; }
        public ProgressStatus Status { get; set; } = null!;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}