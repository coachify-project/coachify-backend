namespace Coachify.API.DTOs.Coach;

public class UpdateCoachDto
{
    public int CoachId { get; set; }
    public int UserId { get; set; }
    public string Bio { get; set; }
}