using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coachify.DAL.Entities;

public class Skill
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int SkillId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;

    public ICollection<Module> Modules { get; set; } = new List<Module>();
}