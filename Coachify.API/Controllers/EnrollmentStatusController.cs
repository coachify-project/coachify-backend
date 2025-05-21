using Coachify.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Coachify.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EnrollmentStatusController : ControllerBase
{
    private readonly IEnrollmentStatusService _service;

    public EnrollmentStatusController(IEnrollmentStatusService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var statuses = await _service.GetAllAsync();
        return Ok(statuses);
    }
}