namespace Coachify.BLL.DTOs.UserCoachApplicationStatus;

public class CreateUserCoachApplicationStatusDto
{
    public int UserId { get; set; }
    public int CoachId { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime ApplicationDate { get; set; }
}