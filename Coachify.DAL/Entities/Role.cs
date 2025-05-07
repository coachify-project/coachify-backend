using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coachify.DAL.Entities;

public class Role
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int RoleId { get; set; }

    [Required, MaxLength(25)] public string RoleName { get; set; } = null!; // Guest, Client, CoachApplicant, Coach, Admin

    public ICollection<User> Users { get; set; } = new List<User>();
}