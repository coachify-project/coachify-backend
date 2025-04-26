using System.ComponentModel.DataAnnotations;

namespace Coachify.DAL.Entities;

public class Role
{
    [Key]
    public int RoleId { get; set; }
    [Required]
    public string RoleName { get; set; } // Guest, Client, CoachApplicant, Coach, Admin

    public ICollection<User> Users { get; set; }
}