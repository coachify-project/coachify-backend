namespace Coachify.BLL.DTOs.Feedback;

public class CreateFeedbackDto
{
    public int CourseId { get; set; }
    public int UserId { get; set; }
    public int StatusId { get; set; }
    public string Text { get; set; }
    public int Rating { get; set; }
}