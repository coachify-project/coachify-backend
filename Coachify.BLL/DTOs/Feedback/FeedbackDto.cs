namespace Coachify.API.DTOs.Feedback;

public class FeedbackDto
{
    public int Id { get; set; }
    public int CourseId { get; set; }
    public int ClientId { get; set; }
    public int StatusId { get; set; }
    public string Comment { get; set; }
}