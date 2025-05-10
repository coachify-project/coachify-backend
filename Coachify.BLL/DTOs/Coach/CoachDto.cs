namespace Coachify.BLL.DTOs.Coach;

public class CoachDto
{
    public int CoachId { get; set; }
    
    public int UserId   => CoachId;
    public string Bio { get; set; }
    public object Specialization { get; set; }
    public object Verified { get; set; }
}