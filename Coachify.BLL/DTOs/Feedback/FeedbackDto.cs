namespace Coachify.BLL.DTOs.Feedback;

public class FeedbackDto
{
    public int Id { get; set; }
    public int CourseId { get; set; }
    public int UserId { get; set; }
    public int StatusId { get; set; }
    public string Text { get; set; }
    public int? Rating { get; set; } = 0;
    
    public ICollection<FeedbackDto> Feedbacks { get; set; } = new List<FeedbackDto>();

}