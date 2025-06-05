using Coachify.DAL.Entities;

namespace Coachify.BLL.DTOs.CoachApplication
{
    public class CoachApplicationDto
    {
        public int ApplicationId { get; set; }
        public int UserId { get; set; }
        public string Bio { get; set; }
        public string Specialization { get; set; }
        public string? AvatarUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public string StatusName { get; set; } = null!;
    }
}