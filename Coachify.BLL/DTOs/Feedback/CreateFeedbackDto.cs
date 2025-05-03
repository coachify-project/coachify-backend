namespace Coachify.API.DTOs.Feedback;

public class CreateFeedbackDto
{
    public int CourseId { get; set; }
    public int ClientId { get; set; }
    public int StatusId { get; set; }
    public string Comment { get; set; }
    public int Rating { get; set; }
}