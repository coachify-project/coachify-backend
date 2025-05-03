namespace Coachify.API.DTOs.Certificate;

public class UpdateCertificateDto
{
    public int EnrollmentId { get; set; }
    public DateTime IssuedDate { get; set; }
}