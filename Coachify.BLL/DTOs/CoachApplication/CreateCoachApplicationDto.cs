namespace Coachify.BLL.DTOs.CoachApplication
{
    public class CreateCoachApplicationDto
    {
        public int UserId { get; set; }
        public string Bio { get; set; }
        public string Specialization { get; set; }
        //public int StatusId { get; set; } = 1;
    }
}