namespace Coachify.BLL.DTOs.UserCoachApplicationStatus;

public class UserCoachApplicationStatusDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int CoachId { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime ApplicationDate { get; set; }
}