using System.ComponentModel.DataAnnotations;

namespace Coachify.BLL.DTOs.Coach;

public class UpdateCoachDto
{
   
    public string Bio { get; set; }
    public object Specialization { get; set; }

    [Url]
    public string AvatarUrl { get; set; }
}