namespace Coachify.BLL.DTOs.CoachApplication
{
    public class CoachApplicationDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string MotivationText { get; set; }
        public DateTime CreatedAt { get; set; }
        public int StatusId { get; set; }
    }
}