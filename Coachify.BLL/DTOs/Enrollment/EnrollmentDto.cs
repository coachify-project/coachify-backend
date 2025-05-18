namespace Coachify.BLL.DTOs.Enrollment;

public class EnrollmentDto
{
    public int EnrollmentId { get; set; }
    public int UserId { get; set; }
    public int CourseId { get; set; }
    public int StatusId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    public string? CourseTitle { get; set; }
    public DateTime EnrolledAt { get; set; }
}