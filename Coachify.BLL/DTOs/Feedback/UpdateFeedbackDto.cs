namespace Coachify.BLL.DTOs.Feedback;

public class UpdateFeedbackDto
{
    public int StatusId { get; set; }
    public string Comment { get; set; }
    public int Rating { get; set; }
}