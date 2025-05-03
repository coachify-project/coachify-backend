namespace Coachify.API.DTOs.Enrollment;

public class CreateEnrollmentDto
{
    public int UserId { get; set; }
    public int CourseId { get; set; }
    public DateTime EnrolledAt { get; set; }
}