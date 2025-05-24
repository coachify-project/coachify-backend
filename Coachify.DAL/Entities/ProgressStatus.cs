using System.ComponentModel.DataAnnotations;

namespace Coachify.DAL.Entities
{
    public class ProgressStatus
    {
        [Key]
        public int StatusId { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; } = null!;
    }
}