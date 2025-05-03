namespace Coachify.API.DTOs.Certificate;

public class CreateCertificateDto
{
    public int EnrollmentId { get; set; }
    public DateTime IssuedDate { get; set; }
}