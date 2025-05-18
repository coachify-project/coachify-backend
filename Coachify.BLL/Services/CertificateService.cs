using AutoMapper;
using Coachify.BLL.DTOs.Certificate;
using Coachify.BLL.Interfaces;
using Coachify.DAL;
using Coachify.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System.IO;
using System.Threading.Tasks;

namespace Coachify.BLL.Services;

public class CertificateService : ICertificateService
{
    private readonly ApplicationDbContext _db;
    private readonly string _certificatesFolder;

    public CertificateService(ApplicationDbContext db)
    {
        _db = db;
        _certificatesFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "certificates");

        if (!Directory.Exists(_certificatesFolder))
            Directory.CreateDirectory(_certificatesFolder);
    }

    public async Task CreateCertificateForEnrollmentAsync(int enrollmentId)
    {
        // Проверяем, есть ли сертификат уже
        var existingCertificate = await _db.Certificates.FirstOrDefaultAsync(c => c.EnrollmentId == enrollmentId);
        if (existingCertificate != null)
            return; // Сертификат уже есть, можно по желанию обновлять или игнорировать

        var enrollment = await _db.Enrollments
            .Include(e => e.User)    // предполагается, что есть навигация User
            .Include(e => e.Course)
            .FirstOrDefaultAsync(e => e.EnrollmentId == enrollmentId);

        if (enrollment == null)
            throw new KeyNotFoundException($"Enrollment with id={enrollmentId} not found");

        var certificate = new Certificate
        {
            EnrollmentId = enrollmentId,
            IssueDate = System.DateTime.UtcNow
        };

        _db.Certificates.Add(certificate);
        await _db.SaveChangesAsync();

        // Генерируем PDF после сохранения, т.к. нужен certificateId
        await GenerateCertificatePdfAsync(certificate.CertificateId, enrollment.User.FirstName,enrollment.User.LastName, enrollment.Course.Title, certificate.IssueDate);
    }

    public async Task<string> GenerateCertificatePdfAsync(int certificateId, string FirstName,string LastName, string courseTitle, System.DateTime issuedAt)
    {
        string fileName = $"certificate_{certificateId}.pdf";
        string filePath = Path.Combine(_certificatesFolder, fileName);

        using (var document = new PdfDocument())
        {
            var page = document.AddPage();
            var gfx = XGraphics.FromPdfPage(page);

            var titleFont = new XFont("Verdana", 24, XFontStyle.Bold);
            var subtitleFont = new XFont("Verdana", 16, XFontStyle.Regular);
            var textFont = new XFont("Verdana", 14, XFontStyle.Regular);

            gfx.DrawString("Certificate of Completion", titleFont, XBrushes.Black,
                new XRect(0, 80, page.Width, 40), XStringFormats.Center);

            gfx.DrawString($"This certifies that", subtitleFont, XBrushes.Black,
                new XRect(0, 140, page.Width, 30), XStringFormats.Center);

            gfx.DrawString(FirstName, new XFont("Verdana", 20, XFontStyle.Bold), XBrushes.Black,
                new XRect(0, 170, page.Width, 40), XStringFormats.Center);
            
            gfx.DrawString(LastName, new XFont("Verdana", 20, XFontStyle.Bold), XBrushes.Black,
                new XRect(0, 170, page.Width, 40), XStringFormats.Center);

            gfx.DrawString($"has successfully completed the course", subtitleFont, XBrushes.Black,
                new XRect(0, 220, page.Width, 30), XStringFormats.Center);

            gfx.DrawString(courseTitle, new XFont("Verdana", 18, XFontStyle.BoldItalic), XBrushes.Black,
                new XRect(0, 250, page.Width, 40), XStringFormats.Center);

            gfx.DrawString($"Date: {issuedAt:dd MMMM yyyy}", textFont, XBrushes.Black,
                new XRect(0, 310, page.Width, 30), XStringFormats.Center);

            gfx.DrawString($"Certificate ID: {certificateId}", textFont, XBrushes.Black,
                new XRect(0, 340, page.Width, 30), XStringFormats.Center);

            document.Save(filePath);
        }

        return $"/certificates/{fileName}";
    }

    public async Task<byte[]> GetCertificatePdfAsync(int certificateId)
    {
        string fileName = $"certificate_{certificateId}.pdf";
        string filePath = Path.Combine(_certificatesFolder, fileName);

        if (!File.Exists(filePath))
            return null;

        return await File.ReadAllBytesAsync(filePath);
    }
}
