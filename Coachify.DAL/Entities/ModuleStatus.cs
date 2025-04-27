using System.ComponentModel.DataAnnotations;

namespace Coachify.DAL.Entities;

public class ModuleStatus
{
    [Key] public int StatusId { get; set; }
    [Required, MaxLength(25)] public string StatusName { get; set; } = null!;

    public ICollection<Module> Modules { get; set; } = new List<Module>();
}