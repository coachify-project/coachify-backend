using Coachify.BLL.DTOs.CoachApplication;
using Coachify.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Coachify.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CoachApplicationsController : ControllerBase
{
    private readonly ICoachApplicationService _service;

    public CoachApplicationsController(ICoachApplicationService service) 
        => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll() 
        => Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var d = await _service.GetByIdAsync(id);
        return d == null ? NotFound() : Ok(d);
    }

    [HttpGet("pending")]
    public async Task<IActionResult> GetPendingApplications()
    {
        var pendingApplications = await _service.GetPendingApplicationsAsync();
        return Ok(pendingApplications);
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(CreateCoachApplicationDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var c = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(Get), new { id = c.ApplicationId }, c);
    }

    [HttpPost("approve/{applicationId}")]
    public async Task<IActionResult> ApproveCoachApplication(int applicationId)
    {
        try
        {
            await _service.ApproveCoachApplicationAsync(applicationId);
            return Ok(new { message = "Application approved successfully" });
            // или return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    
    [HttpPost("reject/{applicationId}")]
    public async Task<IActionResult> RejectCoachApplication(int applicationId)
    {
        try
        {
            await _service.RejectCoachApplicationAsync(applicationId);
            return Ok(new { message = "Application rejected successfully" });
            // или return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id) 
        => Ok(await _service.DeleteAsync(id));
}
