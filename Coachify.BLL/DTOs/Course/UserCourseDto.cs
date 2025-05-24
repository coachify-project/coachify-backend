namespace Coachify.BLL.DTOs.Course;

public class UserCourseDto
{
    public int CourseId { get; set; }
    public string Title { get; set; } = null!;
    public int CoachId { get; set; }
    public int CategoryId { get; set; }
    public int EnrollmentStatusId { get; set; }
    public int ProgressPercentage { get; set; }
    public bool IsEnrolled { get; set; }
}