using Coachify.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Coachify.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CertificateController : ControllerBase
{
    private readonly ICertificateService _certificateService;

    public CertificateController(ICertificateService certificateService)
    {
        _certificateService = certificateService;
    }

    // Метод для генерации сертификата и получения ссылки на PDF
    [HttpPost("generate/{certificateId}")]
    public async Task<IActionResult> GenerateCertificate(int certificateId, string FirstName,string LastName, string courseTitle, System.DateTime issuedAt)
    {
        var url = await _certificateService.GenerateCertificatePdfAsync(certificateId, FirstName,LastName, courseTitle, issuedAt);
        if (string.IsNullOrEmpty(url))
            return BadRequest("Failed to generate certificate");

        return Ok(new { certificateUrl = url });
    }

    // Метод для скачивания PDF файла сертификата по ID
    [HttpGet("{certificateId}")]
    public async Task<IActionResult> GetCertificate(int certificateId)
    {
        var pdfBytes = await _certificateService.GetCertificatePdfAsync(certificateId);
        if (pdfBytes == null)
            return NotFound("Certificate not found");

        return File(pdfBytes, "application/pdf", $"certificate_{certificateId}.pdf");
    }
}


//
// [ApiController]
// [Route("api/[controller]")]
// public class CertificatesController : ControllerBase
// {
//     private readonly ICertificateService _service;
//     public CertificatesController(ICertificateService service) => _service = service;
//
//     [HttpGet]
//     public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());
//
//     [HttpGet("{id}")]
//     public async Task<IActionResult> Get(int id)
//     {
//         var d = await _service.GetByIdAsync(id);
//         return d == null ? NotFound() : Ok(d);
//     }
//
//     [HttpPost]
//     public async Task<IActionResult> Create(CreateCertificateDto dto)
//     {
//         var c = await _service.CreateAsync(dto);
//         return CreatedAtAction(nameof(Get), new { id = c.Id }, c);
//     }
//
//     [HttpPut("{id}")]
//     public async Task<IActionResult> Update(int id, UpdateCertificateDto dto)
//     {
//         await _service.UpdateAsync(id, dto);
//         return NoContent();
//     }
//
//     [HttpDelete("{id}")]
//     public async Task<IActionResult> Delete(int id) => Ok(await _service.DeleteAsync(id));
// }